using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Radical.CQRS.Client
{
	public class CommandClient
	{
		String baseAddress;

		public CommandClient( String baseAddress )
		{
			this.baseAddress = baseAddress;
		}

		public async Task<TResult> ExecuteAsync<TResult>( Object command )
		{
			HttpClient client = new HttpClient();
			var content = new StringContent( JsonConvert.SerializeObject( command ) );
			content.Headers.ContentType = new MediaTypeHeaderValue( "application/json" );

			var url = this.baseAddress + "api/jason/" + command.GetType().Name.ToLowerInvariant();

			var response = await client.PostAsync( url, content );
			var result = await response.Content.ReadAsStringAsync();

			return JsonConvert.DeserializeObject<TResult>( result );
		}
	}
}
