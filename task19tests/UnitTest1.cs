using ServerThreadSystem;
namespace task18tests
{
    public class ServerThreadSystemTests
    {
        [Fact]
        public void ServerWithHardStopWorksCorrect()
        {
            var output = new StringWriter();
            Console.SetOut(output);

            Program.Main();

            for (int i  = 1; i <= 5; i++)
            {
                for (int j = 1; j <= 3; j++)
                {
                    Assert.Contains($"Поток {i} вызов {j}", output.ToString());
                }
            }
        }
    }
}