<Query Kind="Program" />

void Main()
{
	//var rnd = new Random();
	//var nros = new List<int>();
	//var qtdNumeros = 6;
	//var nroMaximo = 60;
	//
	//while (nros.Count != qtdNumeros) {
	//	var n = rnd.Next(nroMaximo);
	//	
	//	if (nros.Any(x => x == n || x == 0))
	//		continue;
	//		
	//	nros.Add(n);
	//}
	//
	//string.Join("-", nros.OrderBy(x => x).ToArray()).Dump();
	
	var minhaAposta = Aposta.Instancia().Gerar(6, 60);
	minhaAposta.ToString().Dump();
	
	var outraAposta = Aposta.Instancia().Gerar(6, 60);
	outraAposta.ToString().Dump();
}

// You can define other methods, fields, classes and namespaces here
class Bolao {
	public int Id { get; set; }
	public string Nome { get; set; }
	public List<Pessoa> Pessoas { get; set; }
	public List<Aposta> Apostas { get; set; }
}

class Pessoa {
	public int Id { get; set; }
	public int Nome { get; set; }
}

class Aposta {
	private List<int> _numeros;
	private static Aposta _instancia = null;
	private const string SEPARADOR_RESULTADO = "-";
	
	private Aposta() {
		_numeros = new List<int>();
	}
	
	public static Aposta Instancia() {
		if (_instancia == null)
			return new Aposta();
			
		return _instancia;
	}
	
	public Aposta Gerar(int qtdNumeros, int nroMaximo)
	{
		var rnd = new Random();
		
		while (_numeros.Count != qtdNumeros) {
			var n = rnd.Next(nroMaximo);
			
			if (_numeros.Any(x => x == n || x == 0))
				continue;
				
			_numeros.Add(n);
		}
		
		return this;
	}
	
	public override string ToString() 
		=> string.Join(SEPARADOR_RESULTADO, _numeros.OrderBy(x => x).ToArray());
}
