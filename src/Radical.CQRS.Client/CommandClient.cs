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
		String correlationIdHeaderName;

		public CommandClient( String baseAddress, String correlationIdHeaderName = "x-jason-correlation-id" )
		{
			this.baseAddress = baseAddress;
			this.correlationIdHeaderName = correlationIdHeaderName;
		}

		public async Task<TResult> ExecuteAsync<TResult>( String correlationId, Object command )
		{
			HttpClient client = new HttpClient();
			var content = new StringContent( JsonConvert.SerializeObject( command ) );
			content.Headers.ContentType = new MediaTypeHeaderValue( "application/json" );
			content.Headers.Add( this.correlationIdHeaderName, correlationId );

			var url = this.baseAddress + "api/jason/" + command.GetType().Name.ToLowerInvariant();

			var response = await client.PostAsync( url, content );
			var result = await response.Content.ReadAsStringAsync();

			return JsonConvert.DeserializeObject<TResult>( result );
		}
	}
}
