using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
namespace csharp_selenium_browserstack
{
    class SingleTest
    {
        public static void execute()
        {
            // Update your credentials
            String? BROWSERSTACK_BUILD_NAME = Environment.GetEnvironmentVariable("BROWSERSTACK_BUILD_NAME");
            if (BROWSERSTACK_BUILD_NAME is null)
                BROWSERSTACK_BUILD_NAME = "areliadan_SrAwum";

            String? BROWSERSTACK_ACCESS_KEY = Environment.GetEnvironmentVariable("BROWSERSTACK_ACCESS_KEY");
            if (BROWSERSTACK_ACCESS_KEY is null)
                BROWSERSTACK_ACCESS_KEY = "89EvungAax7ELsz4tqyi";
            IWebDriver driver;
            SafariOptions capabilities = new SafariOptions();
            Dictionary<string, object> browserstackOptions = new Dictionary<string, object>();
            browserstackOptions.Add("osVersion", "14");
            browserstackOptions.Add("deviceName", "iPhone 12");
            browserstackOptions.Add("realMobile", "true");
            browserstackOptions.Add("local", "false");
            browserstackOptions.Add("buildName", "browserstack-build-1");
            browserstackOptions.Add("userName", BROWSERSTACK_BUILD_NAME);
            browserstackOptions.Add("accessKey", BROWSERSTACK_ACCESS_KEY);
            capabilities.AddAdditionalOption("bstack:options", browserstackOptions);
            //Web application ()
            driver = new RemoteWebDriver(new Uri("https://hub.browserstack.com/wd/hub/"), capabilities);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            try
            {
                driver.Navigate().GoToUrl("https://www.google.com");
                // getting name of the product
                String product_on_page = wait.Until(driver => driver.FindElement(By.XPath("//*[@id=\"1\"]/p"))).Text;
                // clicking the 'Add to Cart' button
                IWebElement cart_btn = driver.FindElement(By.XPath("//*[@id='1']/div[4]"));
                wait.Until(driver => cart_btn.Displayed);
                cart_btn.Click();
                // waiting for the Cart pane to appear
                wait.Until(driver => driver.FindElement(By.ClassName("float-cart__content")).Displayed);
                // getting name of the product in the cart
                String product_in_cart = driver.FindElement(By.XPath("//*[@id='__next']/div/div/div[2]/div[2]/div[2]/div/div[3]/p[1]")).Text;
                if (product_on_page == product_in_cart)
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"passed\", \"reason\": \" Product has been successfully added to the cart!\"}}");
                }
            }
            catch
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"failed\", \"reason\": \" Some elements failed to load.\"}}");
            }
            driver.Quit();
        }
    }
}
