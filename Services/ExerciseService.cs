using Newtonsoft.Json;
using System.Net.Http.Json;


public class ExerciseService
{
    private readonly HttpClient _httpClient;
    public ExerciseService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Key", "03377d1198msh8b4f97fbb46ea41p1c3c79jsncdd342596b81");
        _httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Host", "exercises-by-api-ninjas.p.rapidapi.com");
    }

    // Update the FetchExercisesByMuscleGroup method to be reusable
    public async Task<List<Exercise>> FetchExercisesByMuscleGroup(string muscleGroup)
    {
        var url = $"https://exercises-by-api-ninjas.p.rapidapi.com/v1/exercises?muscle={muscleGroup}";

        try
        {
            var response = await _httpClient.GetFromJsonAsync<List<Exercise>>(url);
            return response ?? new List<Exercise>();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error fetching exercises: " + ex.Message);
            return new List<Exercise>();
        }
    }


}

    
    public class Exercise
{
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? Muscle { get; set; }
    public string? Equipment { get; set; }
    public string? Difficulty { get; set; }
    public string? Instructions { get; set; }
}