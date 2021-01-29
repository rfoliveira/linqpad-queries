<Query Kind="Program">
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main()
{
	//var dir = @"C:\cursos\microsoft\webapi-with-litedb\WebAPIWithLiteDB";
	//var dir = @"C:\cursos\microsoft";
	//var busca = new[] {"Swagger", "Weather", "use", "microSOFT"};
	//var resultado = new ConcurrentDictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
	
	//var linhas = File.ReadAllLines(@"C:\cursos\microsoft\webapi-with-litedb\WebAPIWithLiteDB\Program.cs");
	//var resultado = new ConcurrentDictionary<string>();
	/*
	Parallel.ForEach(
		new DirectoryInfo(dir)
		.GetFiles()
		//.AsParallel()
		.Where(f => f.Extension.Equals(".cs")),
		(arq) => {
			var linhas = File.ReadAllLines(arq.FullName);			
			
			// A cada linha...
			//linhas.Dump($"Linhas - {arq.FullName}");
			
			Parallel.ForEach(linhas, l => {
				if (!string.IsNullOrEmpty(l))
				{			
					var palavras = l.Trim().Split(' ');	
					
					Parallel.ForEach(palavras, p => {
						
						// A cada palavra da linha...
						Parallel.ForEach(
							busca, 
							b => {
								var qtd = Array.FindAll(busca, b => b.Contains(p, StringComparison.InvariantCultureIgnoreCase))
									.Count();
							//
							//	qtd.Dump($"b = {b}, p = {p}, qtd = {qtd}");
								
								//$"b = {b}, p = {p}".Dump();
								// A cada item da busca...
								//if (qtd > 0) {
								if (p.Contains(b, StringComparison.InvariantCultureIgnoreCase)) {
									$"b = {b}, p = {p}".Dump("Added");
									resultado.AddOrUpdate(arq.FullName + "->" + b, qtd, (k, v) => v++);
								}
							}
						);
						
						//if (Array.FindIndex(busca, x => p.Contains(x)) > 0) {
						//	resultado.AddOrUpdate(
						//		arq.FullName, 
						//		qtdPalavrasLinha, 
						//		(key, oldVal) => Interlocked.Increment(ref qtdPalavrasLinha)
						//	);
						//}
					});
				}
			});
		}
	);
	
	resultado.Keys.Dump("Resultado");
	*/
	
	var dir = @"C:\cursos\microsoft";
	var busca = new[] {"Swagger", "Weather", "use", "microSOFT"};
	var helper = new DirHelper(busca, new [] { ".cs", ".config" });
	
	helper.LeDiretorios(dir).ObterResultado().Dump();
}

// You can define other methods, fields, classes and namespaces here
class DirHelper {
	
	private string[] _pesquisa;
	private string[] _extensoes;
	private ConcurrentDictionary<string, int> _resultado;
	private const string FORMATO_SAIDA_PADRAO = "Arquivo: {0}, palavra: {1}, qtd: {2}";

	public DirHelper(string[] pesquisa, string[] extensoes) {
		_pesquisa = pesquisa;
		_extensoes = extensoes;		
		_resultado = new ConcurrentDictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
	}
	
	public DirHelper LeDiretorios(string dirPath) {
		var dirs = new DirectoryInfo(dirPath).GetDirectories().AsParallel();
		
		Parallel.ForEach(dirs, d => {
			LeDiretorios(d.FullName);
		});
		
		//var files = new DirectoryInfo(dirPath).GetFiles().Where(f => f.Extension == ".cs").AsParallel();
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
	
	public string ObterResultado(string formato = FORMATO_SAIDA_PADRAO) {
		var retorno = string.Empty;
		foreach (var key in _resultado.Keys) {
			var arquivo_chave = key.Split("->");
			retorno += string.Format(formato + "\n", arquivo_chave[0], arquivo_chave[1], _resultado[key]);
		}
			
		return retorno;
	}
}
