﻿// Licensed under the GPL License, Version 3.0. See LICENSE in the git repository root for license information.

using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;

namespace Ecwid
{
	/// <summary>
	/// Abstract client class contains shared methods.
	/// </summary>
	public abstract class BaseEcwidClient
	{
		/// <summary>
		/// Checks the shop authentication asynchronous.
		/// </summary>
		/// <exception cref="EcwidHttpException">Something happened to the HTTP call.</exception>
		protected static async Task<bool> CheckTokenAsync<T>(Url url, CancellationToken cancellationToken)
			where T : class
		{
			try
			{
				await GetApiAsync<T>(url, new {limit = 1}, cancellationToken);
				return true;
			}
			catch (EcwidHttpException exception)
			{
				var status = exception.StatusCode;
				if (status == HttpStatusCode.Forbidden)
					return false;

				throw;
			}
		}

		/// <summary>
		/// GET from API asynchronous.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="baseUrl">The base URL.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <exception cref="EcwidHttpException">Something happened to the HTTP call.</exception>
		protected static async Task<T> GetApiAsync<T>(Url baseUrl, CancellationToken cancellationToken)
			where T : class
		{
			T poco;
			try
			{
				poco = await baseUrl.GetJsonAsync<T>(cancellationToken);
			}
			catch (FlurlHttpException exception)
			{
				var call = exception.Call;
				var status = call.Response?.StatusCode;

				// if entity not found return null
				if (status == HttpStatusCode.NotFound)
					return null;

				var error = call.ErrorResponseBody ?? call.Exception?.Message ?? "Something happened to the HTTP call.";

				throw new EcwidHttpException(error, status, exception);
			}

			return poco;
		}

		/// <summary>
		/// GET from API asynchronous.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="baseUrl">The base URL.</param>
		/// <param name="query">The query.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <exception cref="EcwidHttpException">Something happened to the HTTP call.</exception>
		protected static async Task<T> GetApiAsync<T>(Url baseUrl, object query,
			CancellationToken cancellationToken)
			where T : class
			=> await GetApiAsync<T>(baseUrl.SetQueryParams(query), cancellationToken);

		/// <summary>
		/// POST the API asynchronous and return response.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="baseUrl">The base URL.</param>
		/// <param name="query">The query.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <exception cref="EcwidHttpException">Something happened to the HTTP call.</exception>
		protected static async Task<T> PostApiAsync<T>(Url baseUrl, object query, CancellationToken cancellationToken)
			where T : class
		{
			T poco;
			try
			{
				poco = await baseUrl.SetQueryParams(query).PostAsync(null, cancellationToken).ReceiveJson<T>();
			}
			catch (FlurlHttpException exception)
			{
				var call = exception.Call;
				var status = call.Response?.StatusCode;
				var error = call.ErrorResponseBody ?? call.Exception?.Message ?? "Something happened to the HTTP call.";

				throw new EcwidHttpException(error, status, exception);
			}

			return poco;
		}

		/// <summary>
		/// PUT the API asynchronous and return response.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="baseUrl">The base URL.</param>
		/// <param name="data">The new object.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <exception cref="EcwidHttpException">Something happened to the HTTP call.</exception>
		protected static async Task<T> PutApiAsync<T>(Url baseUrl, object data, CancellationToken cancellationToken)
			where T : class
		{
			T poco;
			try
			{
				poco = await baseUrl.PutJsonAsync(data, cancellationToken).ReceiveJson<T>();
			}
			catch (FlurlHttpException exception)
			{
				var call = exception.Call;
				var status = call.Response?.StatusCode;
				var error = call.ErrorResponseBody ?? call.Exception?.Message ?? "Something happened to the HTTP call.";

				throw new EcwidHttpException(error, status, exception);
			}

			return poco;
		}

		/// <summary>
		/// PUT the API asynchronous and return response.
		/// </summary>
		/// <param name="baseUrl">The base URL.</param>
		/// <param name="query">The query.</param>
		/// <param name="data">The new object.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <exception cref="EcwidHttpException">Something happened to the HTTP call.</exception>
		protected static async Task<bool> PutApiAsync(Url baseUrl, object query, object data, CancellationToken cancellationToken)
		{
			try
			{
				await baseUrl.SetQueryParams(query).PutJsonAsync(data, cancellationToken).ReceiveJson();
			}
			catch (FlurlHttpException exception)
			{
				var call = exception.Call;
				var status = call.Response?.StatusCode;
				var error = call.ErrorResponseBody ?? call.Exception?.Message ?? "Something happened to the HTTP call.";

				throw new EcwidHttpException(error, status, exception);
			}

			return true;
		}
	}
}