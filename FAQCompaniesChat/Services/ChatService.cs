using FAQCompaniesChat.Data;
using Microsoft.Bot.Connector.DirectLine;

namespace FAQCompaniesChat.Services
{
    public class ChatService
    {
        private readonly IConfiguration _configuration;
        DirectLineClient? client;
        Conversation? conversation;

        public ChatService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task InitConversation()
        {
            var chatSecret = _configuration.GetValue<string>("BotService:ChatSecret");
            client = new DirectLineClient(chatSecret);
            conversation = await client.Conversations.StartConversationAsync();
        }

        public async Task<Message> GetResponse(List<Message> messageChain)
        {
            Message? responseMessage = null;
            Activity userMessage = new Activity
            {
                From = new ChannelAccount("DirectLineFaqCompanyUser"),
                Text = messageChain.Last().Body,
                Type = ActivityTypes.Message
            };

            if(client is null || conversation is null) {
                responseMessage = new Message("No response.", false);
                return responseMessage;
            }

            var postActivityResponse = await client.Conversations.PostActivityAsync(conversation.ConversationId, userMessage);
            var activitiesSet = await client.Conversations.GetActivitiesAsync(conversation.ConversationId);
            var botResponses = activitiesSet.Activities.Where(a => a.ReplyToId == postActivityResponse.Id);

            foreach (Activity activity in botResponses)
            {
                if (activity.Text != null)
                {
                    responseMessage = new Message(activity.Text, false);
                }
            }

            if (responseMessage == null)
            {
                responseMessage = new Message("No response.", false);
            }
            return responseMessage;
        }     
    }
}