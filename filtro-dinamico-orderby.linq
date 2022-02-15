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
	//var p = new Pessoa { Nome = "Rafael", TipoPessoa = PessoaEnum.PFISICA };
	//$"Tipo da pessoa {p.TipoPessoa.ToString()}".Dump();
	
	var l = new List<Pessoa>();
	
	for (int i = 0; i < 10; i++) {
		if (i % 2 == 0) 
			l.Add(new Pessoa { Nome = $"{i + 1} - Pessoa", TipoPessoa = PessoaEnum.PFISICA });
		else 
			l.Add(new Pessoa { Nome = $"{i + 1} - Pessoa", TipoPessoa = PessoaEnum.PJURIDICA });
	}
	
	var ordernarPor = "Nome";
	var propertyInfo = typeof(Pessoa).GetProperty(ordernarPor);
	var orderByPFisica = l.OrderBy(x => propertyInfo.GetValue(x, null));
	
	orderByPFisica.Dump();	
	
	l.Dump();
}

// You can define other methods, fields, classes and namespaces here
enum PessoaEnum {
	[Description("Física")]
	PFISICA,
	
	[Description("Jurídica")]
	PJURIDICA
}
	
class Pessoa {
	public PessoaEnum? TipoPessoa { get; set; }
	public string Nome { get; set; }
}