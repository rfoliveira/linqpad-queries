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
	var rnd = new Random();
	var nros = new List<int>();
	var qtdNumeros = 6;
	
	while (nros.Count != qtdNumeros) {
		var n = rnd.Next(60);
		
		if (nros.Any(x => x == n))
			continue;
			
		nros.Add(n);
	}
	
	nros.OrderBy(x => x).Dump();
}

// You can define other methods, fields, classes and namespaces here