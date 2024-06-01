using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using dotenv.net;

namespace TS_BOT
{
    internal class Program 
    {
        private readonly DiscordSocketClient client;
        private readonly string? dc_token; //Token from .env file

        static Program()
        {
            // Load the .env file once at the start of the program.
            DotEnv.Load();
        }

        public Program()
        {
            this.dc_token = Environment.GetEnvironmentVariable("DISCORD_TOKEN")?.Trim();
            if (string.IsNullOrEmpty(this.dc_token))
            {
                Console.WriteLine("The DISCORD_TOKEN environment variable is not set or is empty.");
                throw new InvalidOperationException("Bot token is not set.");
            }

            this.client = new DiscordSocketClient();
            this.client.MessageReceived += MessageHandler;
        }
        //Message Response
        private async Task MessageHandler(SocketMessage message)
        {
            if (message.Author.IsBot) return;
            await ReplyAsync(message, "C# Response Works!");
        }
        //Reply Function
        private async Task ReplyAsync(SocketMessage message, string response)
        {
            await message.Channel.SendMessageAsync(response);
        }
        //Bot Login
        public async Task StartBotAsync()
        {
            this.client.Log += LogFuncAsync;

            try
            {
                await this.client.LoginAsync(TokenType.Bot, this.dc_token);
                await this.client.StartAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during login or start: {ex.Message}");
                return;
            }

            await Task.Delay(-1);
        }

        private Task LogFuncAsync(LogMessage message)
        {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }

        static async Task Main(string[] args) 
        {
            var myBot = new Program();
            await myBot.StartBotAsync();
        }
    }
}
