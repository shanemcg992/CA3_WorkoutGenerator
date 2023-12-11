using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using System.Threading.Tasks;
using SeleniumExtras.WaitHelpers;

[TestClass]
public class E2ETests
{
    private IWebDriver driver;
    private WebDriverWait wait;
    private string baseUrl = "http://localhost:5000";

    [TestInitialize]
    public void TestInitialize()
    {
        driver = new ChromeDriver();
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        driver.Navigate().GoToUrl(baseUrl);
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
    }

    [TestCleanup]
    public void TestCleanup()
    {
        driver.Quit();
    }

    [TestMethod]
    public void SearchAndSelectMuscleGroup()
    {
        var searchInput = driver.FindElement(By.ClassName("search-input"));
        searchInput.SendKeys("chest");

        var searchResultItem = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.ClassName("search-result-item")));
        searchResultItem.Click();

        var workoutList = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.ClassName("list-group")));
        var exercises = workoutList.FindElements(By.TagName("button"));

        Assert.IsTrue(exercises.Count > 0, "No exercises listed for the selected muscle group.");
    }

    [TestMethod]
    public void GenerateWorkoutAndCheckList()
    {
        var searchInput = driver.FindElement(By.ClassName("search-input"));
        searchInput.SendKeys("chest");

        var searchResultItem = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.ClassName("search-result-item")));
        searchResultItem.Click();

        var generateButton = driver.FindElement(By.XPath("//button[contains(text(), 'Generate Workout of the Day')]"));
        generateButton.Click();

        var workoutList = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.ClassName("list-group")));
        var exercises = workoutList.FindElements(By.TagName("button"));

        Assert.IsTrue(exercises.Count > 0, "Workout of the Day did not generate any exercises.");
    }

    [TestMethod]
    public void CheckModalOpeningForExerciseDetails()
    {

        var searchInput = driver.FindElement(By.ClassName("search-input"));
        searchInput.SendKeys("chest");

        var searchResultItem = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.ClassName("search-result-item")));
        searchResultItem.Click();

        var exerciseButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.ClassName("list-group-item")));
        exerciseButton.Click();

        var modal = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.ClassName("modal")));
        Assert.IsTrue(modal.Displayed, "Modal for exercise details did not open.");
    }

    [TestMethod]
    public void GenerateAndVerifyWeeklyWorkoutPlan()
    {
        // Find and click on the checkbox for Monday.
        var mondayCheckbox = driver.FindElement(By.Id("Monday"));
        mondayCheckbox.Click();

        var mondayCard = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//h5[contains(text(), 'Monday')]/following-sibling::select")));
        var selectElement = new SelectElement(mondayCard);
        selectElement.SelectByIndex(1);

        // Click the "Generate Weekly Workout Plan" button.
        var generateWeeklyWorkoutButton = driver.FindElement(By.XPath("//button[contains(text(), 'Generate Weekly Workout Plan')]"));
        generateWeeklyWorkoutButton.Click();

        // Wait for the weekly workout plan to appear
        var weeklyWorkoutPlan = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//h2[contains(text(), 'Weekly Workout Plan')]")));
        Assert.IsNotNull(weeklyWorkoutPlan, "Weekly workout plan was not generated.");
    }

}
