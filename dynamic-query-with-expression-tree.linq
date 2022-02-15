<Query Kind="Program">
  <Output>DataGrids</Output>
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

using static Extensions.ListExtensions;
using Domain;

void Main()
{
	//Expression<Func<Pessoa, bool>> ehMaiorDe18 = p => p.Idade > 18;
	//var fulano = new Pessoa { Idade = 22, Nome = "Fulano" };
	//var maior18 = ehMaiorDe18.Compile();
	//maior18(fulano).Dump();
	
	var l = new List<Pessoa>() {
		new Domain.Pessoa { Idade = 11, Nome = "Fulano" },
		new Domain.Pessoa { Idade = 22, Nome = "belano" },
		new Domain.Pessoa { Idade = 33, Nome = "Ciclano" }
	};
	
	var filtro = l.GetFiltro("Nome", "belano");
	filtro.Dump();
	
	var l2 = new List<Product>() {
		new Domain.Product { Id = 1, Description = "Product 1" },
		new Domain.Product { Id = 2, Description = "Product 2" },
		new Domain.Product { Id = 3, Description = "Product 3" }
	};
	
	var fp = l2.GetFiltro("description", "Product 3");
	fp.Dump();
	
	l.AsQueryable().OrderBy("NOME").Dump();
	
	var orderby = GetOrderBy<Pessoa>("nome");
	l.AsQueryable().OrderBy(orderby).Dump();
}

// You can define other methods, fields, classes and namespaces here
namespace Domain {

	public class Pessoa {
		public int Idade { get; set; }
		public string Nome { get; set; }
	}
	
	public class Product {
		public int Id { get; set; }
		public string Description { get; set; }
	}
}

namespace Extensions {
	using Domain;
	
	public static class ListExtensions {

		public static IEnumerable<T> GetFiltro<T>(this IList<T> lista, string campoFiltro, string valor)
		{
			var tipo = Expression.Parameter(typeof(T));
			var memberAccess = Expression.PropertyOrField(tipo, campoFiltro);
			var exprRight = Expression.Constant(valor);
			var equalExpr = Expression.Equal(memberAccess, exprRight);
			
			var lambda = Expression.Lambda<Func<T, bool>>(equalExpr, tipo).Compile();
			
			return lista.Where(lambda).ToList();
		}
		
		public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, string propertyName)
		{
			var entityType = typeof(T);
			var propertyInfo = entityType.GetProperties().Where(p => p.Name.ToLower() == propertyName.ToLower()).FirstOrDefault();
			ParameterExpression arg = Expression.Parameter(entityType, "o"); // o => o.propertyName
			MemberExpression property = Expression.Property(arg, propertyName);
			var selector = Expression.Lambda(property, new ParameterExpression[] { arg });
			
			// Get System.Linq.Queryable.OrderBy() method
			var enumerableType = typeof(System.Linq.Queryable);
			var method = enumerableType.GetMethods()
				.Where(m => m.Name == "OrderBy" && m.IsGenericMethodDefinition)
				.Where(m => {
					var parameters = m.GetParameters().ToList();
					
					// Put more restrictions here to ensure selecting the right overload
					return parameters.Count == 2;	// overload that has 2 parameters
				})
				.Single();
			
			// The linq's OrderBy<TSource, TKey> has two generic types, which provided here
			// Ref.: https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.orderby?view=net-5.0
			MethodInfo genericMethod = method
				.MakeGenericMethod(entityType, propertyInfo.PropertyType);
				
			// Call query.OrderBy(selector), with query and selector: o => o.PropName
			// Note that we pass the selector as Expression to the method and we won't compile it.
			// By doing so EF can extract "order by" columns and generate SQL for it.
			var newQuery = (IOrderedQueryable<T>)genericMethod.Invoke(genericMethod, new object[] { query, selector });
			
			return newQuery;
		}
		
		public static Expression<Func<T, object>> GetOrderBy<T>(string orderByField)
		{
			var param = Expression.Parameter(typeof(T));
			Expression conversion = Expression.Convert(Expression.Property(param, orderByField), typeof(object));
			
			return Expression.Lambda<Func<T, object>>(conversion, param);
		}
		//public static Expression<Func<T, bool>> GetFiltroByObject<T>(IList<T> lista)
		//{
		//	if (lista == null)
		//		return null;
		//		
		//	var tipo = typeof(T);
		//	
		//	foreach (PropertyInfo p in tipo.GetProperties())
		//	{
		//		object value = p.GetValue(tipo, null);
		//		
		//		if (Object.ReferenceEquals(value, null)
		//			continue;
		//			
		//		if (value is string && !string.IsNullOrEmpty((string)value)) {
		//			// setar expression<T, value>
		//			continue;
		//		}
		//		
		//		if (value is int && (int)value > 0) {
		//			// ...
		//			continue;
		//		}
		//		
		//		if (value is decimal && (int)value > 0) {
		//			// ...
		//			continue;
		//		}
		//		
		//		if (value is Guid && (Guid)value != Guid.Empty) {
		//			// ...
		//			continue;
		//		}
		//	}
		//	
		//	// return Expression....
		//}
	}
}

