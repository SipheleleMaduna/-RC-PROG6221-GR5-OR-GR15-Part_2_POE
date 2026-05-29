using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Threading.Tasks;
using sphelele.Services;
using sphelele.Models;

namespace sphelele
{
    public partial class MainWindow : Window
    {
        //private AudioPlayer _audioPlayer;
        private RespondingServices _responseService;
        private UserProfile _userProfile;
        public MainWindow()
        {
            _responseService = new RespondingServices();
            _userProfile = new UserProfile();
            InitializeComponent();
           AddMessage($"{_userProfile.GetArt()}", false);

        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string userMessage = MessageInput.Text;
            if (string.IsNullOrWhiteSpace(userMessage))
                return;


            AddMessage(userMessage, true);


            AddMessage("Thinking...", false);

            await Task.Delay(1500);


            if (chatPanel.Children.Count > 0)
                chatPanel.Children.RemoveAt(chatPanel.Children.Count - 1);


            string botReply = _responseService.GetRespond(userMessage);


            AddMessage(botReply, false);

            MessageInput.Clear();

        }

        private void AddMessage(string message, bool isUserMessage)
        {
            Border messageBorder = new Border
            {
                CornerRadius = new CornerRadius(15),
                Padding = new Thickness(15, 10, 15, 10),
                Margin = isUserMessage ? new Thickness(100, 0, 0, 10) : new Thickness(0, 0, 100, 10),
                HorizontalAlignment = isUserMessage ? HorizontalAlignment.Right : HorizontalAlignment.Left
            };

            // Set background color based on message type
            messageBorder.Background = isUserMessage ?
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF4CAF50")) :
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));

            TextBlock messageText = new TextBlock
            {
                Text = message,
                FontSize = 14,
                TextWrapping = TextWrapping.Wrap,
                Foreground = isUserMessage ?
                    new SolidColorBrush(Colors.White) :
                    new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF333333"))
            };

            messageBorder.Child = messageText;
            chatPanel.Children.Add(messageBorder);

        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            AddMessage($"{_userProfile.GetArt()}", false);
            AddMessage("👋 Welcome to CyberBot! ", false);
        }

    }
}