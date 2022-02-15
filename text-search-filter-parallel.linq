<Query Kind="Program">
  <Output>DataGrids</Output>
  <Reference Relative="..\..\Downloads\dlls\Json130r1\Bin\netstandard2.0\Newtonsoft.Json.dll">C:\Users\rfoliveira\Downloads\dlls\Json130r1\Bin\netstandard2.0\Newtonsoft.Json.dll</Reference>
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>System.ComponentModel</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main()
{
	var dir = @"C:\git";
	var busca = new[] {"dinheiro"};
	var helper = new DirHelper(busca, new [] { ".json" }, new[] { "\\eu\\" });
	
	helper.LeDiretorios(dir).ObterResultado().Dump();
}

// You can define other methods, fields, classes and namespaces here
class DirHelper {
	
	private string[] _pesquisa;
	private string[] _extensoes;
	private string[] _diretoriosADesconsiderar;
	private ConcurrentDictionary<string, int> _resultado;
	private const string FORMATO_SAIDA_PADRAO = "Arquivo: {0}, palavra: {1}, qtd: {2}";

	public DirHelper(string[] pesquisa, string[] extensoes) {
		_pesquisa = pesquisa;
		_extensoes = extensoes;		
		_resultado = new ConcurrentDictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
	}
	
	public DirHelper(string[] pesquisa, string[] extensoes, string[] diretoriosADesconsiderar): this(pesquisa, extensoes) {
		_diretoriosADesconsiderar = diretoriosADesconsiderar;
	}
	
	public DirHelper LeDiretorios(string dirPath) {
		if (Array.Exists(_diretoriosADesconsiderar, d => dirPath.Contains(d, StringComparison.InvariantCultureIgnoreCase))) 
			return null;

		var dirs = new DirectoryInfo(dirPath).GetDirectories().AsParallel();
		
		Parallel.ForEach(dirs, d => {
			LeDiretorios(d.FullName);
		});
		
		var files = new DirectoryInfo(dirPath).GetFiles().Where(f => _extensoes.Any(ex => ex.Equals(f.Extension))).AsParallel();
		
		Parallel.ForEach(files, f => {
			LeArquivo(f.FullName);
		});
		
		return this;
	}
	
	private void LeArquivo(string filePath) {
		var linhas = File.ReadAllLines(filePath);
		int qtd = 0;
		
		Parallel.ForEach(linhas, l => {
			if (!string.IsNullOrEmpty(l)) {
				var palavras = l.Split(' ');
				
				Parallel.ForEach(_pesquisa, pe => {
					var qtdAux = Array.FindAll(palavras, pa => pa.Contains(pe, StringComparison.InvariantCultureIgnoreCase)).Count();
					
					if (qtdAux > 0) {
						_resultado.AddOrUpdate(
							$"{filePath}->{pe}", 
							Interlocked.Add(ref qtd, qtdAux), 
							(k, v) => v += qtdAux
						);
					}
				});
			}
		});
	}
	
	public string ObterResultado(string formato = FORMATO_SAIDA_PADRAO, string caminhoArquivoSaida = null) {
		var retorno = string.Empty;
		foreach (var key in _resultado.Keys) {
			var arquivo_chave = key.Split("->");
			retorno += string.Format(formato + "\n", arquivo_chave[0], arquivo_chave[1], _resultado[key]);
		}
		
		if (!string.IsNullOrEmpty(caminhoArquivoSaida))
			File.WriteAllText(caminhoArquivoSaida, retorno);
			
		return retorno;
	}
}