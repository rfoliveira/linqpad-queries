<Query Kind="Program">
  <Output>DataGrids</Output>
  <Reference Relative="..\..\Downloads\dlls\automapper.10.1.1\lib\netstandard2.0\AutoMapper.dll">C:\Users\rfoliveira\Downloads\dlls\automapper.10.1.1\lib\netstandard2.0\AutoMapper.dll</Reference>
  <Reference Relative="..\..\Downloads\documentformat.openxml.2.5.0\lib\DocumentFormat.OpenXml.dll">C:\Users\rfoliveira\Downloads\documentformat.openxml.2.5.0\lib\DocumentFormat.OpenXml.dll</Reference>
  <Reference>C:\git\ativa-portal\DLLs\EPPlus.dll</Reference>
  <Reference Relative="..\..\Downloads\dlls\Json130r1\Bin\netstandard2.0\Newtonsoft.Json.dll">C:\Users\rfoliveira\Downloads\dlls\Json130r1\Bin\netstandard2.0\Newtonsoft.Json.dll</Reference>
  <Namespace>DocumentFormat.OpenXml</Namespace>
  <Namespace>DocumentFormat.OpenXml.Packaging</Namespace>
  <Namespace>DocumentFormat.OpenXml.Spreadsheet</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>OfficeOpenXml</Namespace>
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>System.ComponentModel</Namespace>
  <Namespace>System.IO.Packaging</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Net.Mail</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Web</Namespace>
</Query>

void Main()
{
	var pessoa = new Pessoa {
		Id = 1,
		Nome = "Rafael",
		UF = "RJ",
		Idade = 39,
		IsActive = true
	};
	
	byte[] objByteArr;
	// escrevendo
	using (MemoryStream ms = new MemoryStream()) {
		using (BinaryWriter bw = new BinaryWriter(ms)) {
			bw.Write(pessoa.Id);
			bw.Write(pessoa.Nome);
			bw.Write(pessoa.UF);
			bw.Write(pessoa.Idade);
			bw.Write(pessoa.IsActive);
		}
		
		objByteArr = ms.ToArray();
	}
	
	objByteArr.Dump("escrito");
	
	// lendo
	var strPessoa = new StringBuilder();
	using (MemoryStream ms = new MemoryStream(objByteArr)) {
		using (BinaryReader br = new BinaryReader(ms)) {
			strPessoa.AppendLine($"pessoa.Id = {br.ReadInt32()}");
			strPessoa.AppendLine($"pessoa.Nome = {br.ReadString()}");
			strPessoa.AppendLine($"pessoa.UF = {br.ReadString()}");
			strPessoa.AppendLine($"pessoa.Idade = {br.ReadInt32()}");
			strPessoa.AppendLine($"pessoa.IsActive = {br.ReadBoolean()}");
		}
	}
	
	strPessoa.Dump("lido");
}

// You can define other methods, fields, classes and namespaces here
class Pessoa {
	public int Id { get; set; }
	public string Nome { get; set; }
	public string UF { get; set; }
	public int Idade { get; set; }
	public bool IsActive { get; set; }
}