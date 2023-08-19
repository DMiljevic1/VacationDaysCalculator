using DomainModel.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using DomainModel.DtoModels;

namespace VacationDaysCalculatorBlazorServer.Services
{
	public class SickLeaveService
	{
		private readonly HttpClient _httpClient;
		private readonly CustomAuthenticationStateProvider _customAuthenticationStateProvider;
		private readonly string BaseApiUrl = "https://localhost:7058/api/SickLeave";

		public SickLeaveService(HttpClient httpClient, CustomAuthenticationStateProvider customAuthenticationStateProvider)
		{
			_httpClient = httpClient;
			_customAuthenticationStateProvider = customAuthenticationStateProvider;
		}
		public async Task CloseSickLeaveStatusAsync(SickLeave sickLeave)
		{
			try
			{
				await _customAuthenticationStateProvider.DeleteExpiredTokenFromLocalStorage();
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _customAuthenticationStateProvider.GetTokenAsync());
				var httpPutRequest = new HttpRequestMessage(HttpMethod.Put, $"{BaseApiUrl}/closeSickLeave");
				httpPutRequest.Content = new StringContent(JsonSerializer.Serialize(sickLeave), Encoding.UTF8, "application/json");
				await _httpClient.SendAsync(httpPutRequest);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}

		public async Task<List<MedicalCertificate>> GetMedicalCertificatesAsync(int sickLeaveId)
		{
			try
			{
				await _customAuthenticationStateProvider.DeleteExpiredTokenFromLocalStorage();
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _customAuthenticationStateProvider.GetTokenAsync());
				return await _httpClient.GetFromJsonAsync<List<MedicalCertificate>>($"{BaseApiUrl}/getMedicalCertificates/{sickLeaveId}");
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return null;
			}
		}
		public async Task AddSickLeave(SickLeave sickLeave)
		{
			try
			{
				await _customAuthenticationStateProvider.DeleteExpiredTokenFromLocalStorage();
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _customAuthenticationStateProvider.GetTokenAsync());
				sickLeave.UserId = await _customAuthenticationStateProvider.GetUserId();
				var httpPostRequest = new HttpRequestMessage(HttpMethod.Post, $"{BaseApiUrl}/addSickLeave");
				httpPostRequest.Content = new StringContent(JsonSerializer.Serialize(sickLeave), Encoding.UTF8, "application/json");
				await _httpClient.SendAsync(httpPostRequest);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}
		public async Task UploadMedicialCertificate(MedicalCertificate medicalCertificate)
		{
			try
			{
				await _customAuthenticationStateProvider.DeleteExpiredTokenFromLocalStorage();
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _customAuthenticationStateProvider.GetTokenAsync());
				var httpPostRequest = new HttpRequestMessage(HttpMethod.Post, $"{BaseApiUrl}/uploadMedicalCertificate");
				httpPostRequest.Content = new StringContent(JsonSerializer.Serialize(medicalCertificate), Encoding.UTF8, "application/json");
				await _httpClient.SendAsync(httpPostRequest);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}

		public async Task<MedicalCertificate> GetMedicalCertificateAsync(int medicalCertificateId)
		{
			try
			{
				await _customAuthenticationStateProvider.DeleteExpiredTokenFromLocalStorage();
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _customAuthenticationStateProvider.GetTokenAsync());
				return await _httpClient.GetFromJsonAsync<MedicalCertificate>($"{BaseApiUrl}/getMedicalCertificate/{medicalCertificateId}");
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return null;
			}
		}

		public async Task<List<SickLeave>> GetArchivedSickLeavesByUserIdAsync()
		{
			try
			{
				await _customAuthenticationStateProvider.DeleteExpiredTokenFromLocalStorage();
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _customAuthenticationStateProvider.GetTokenAsync());
				int userId = await _customAuthenticationStateProvider.GetUserId();
				return await _httpClient.GetFromJsonAsync<List<SickLeave>>($"{BaseApiUrl}/getArchivedSickLeavesByUserId/{userId}");
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return null;
			}
		}

		public async Task<List<SickLeave>> GetClosedSickLeavesAsync()
		{
			try
			{
				await _customAuthenticationStateProvider.DeleteExpiredTokenFromLocalStorage();
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _customAuthenticationStateProvider.GetTokenAsync());
				return await _httpClient.GetFromJsonAsync<List<SickLeave>>($"{BaseApiUrl}/getClosedSickLeaves");
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return null;
			}
		}

		public async Task<List<SickLeave>> GetSickLeavesAsync()
		{
			try
			{
				await _customAuthenticationStateProvider.DeleteExpiredTokenFromLocalStorage();
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _customAuthenticationStateProvider.GetTokenAsync());
				return await _httpClient.GetFromJsonAsync<List<SickLeave>>($"{BaseApiUrl}/getSickLeaves");
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return null;
			}
		}

		public async Task ArchiveSickLeaveAsync(SickLeave sickLeave)
		{
			try
			{
				await _customAuthenticationStateProvider.DeleteExpiredTokenFromLocalStorage();
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _customAuthenticationStateProvider.GetTokenAsync());
				var httpPutRequest = new HttpRequestMessage(HttpMethod.Put, $"{BaseApiUrl}/archiveSickLeave");
				httpPutRequest.Content = new StringContent(JsonSerializer.Serialize(sickLeave), Encoding.UTF8, "application/json");
				await _httpClient.SendAsync(httpPutRequest);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}
	}
}
