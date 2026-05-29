using System;
using System.Collections.Generic;
using System.Linq;

namespace sphelele.Services;

public class RespondingServices
{
    private string currentTopic = null;
    private Dictionary<string, List<string>> topicDetails = new Dictionary<string, List<string>>();
    private Dictionary<string, string> shortDefinitions = new Dictionary<string, string>();
    private List<string> conversationHistory = new List<string>();

    public RespondingServices()
    {
        // Initialize short definitions for quick responses
        shortDefinitions.Add("password", "A secret word/phrase used to verify identity and gain access to accounts");
        shortDefinitions.Add("phishing", "A cyberattack where attackers trick you into revealing sensitive information");
        shortDefinitions.Add("safe browsing", "Practicing secure habits while surfing the internet");
    }
    
    public string GetRespond(string respond)
    {
        if (string.IsNullOrWhiteSpace(respond))
            return "Please say something! I'm here to help with cybersecurity topics.";

        // Store conversation history
        conversationHistory.Add(respond);
        if (conversationHistory.Count > 10)
            conversationHistory.RemoveAt(0);

        // Check for "tell me more" request FIRST
        if (respond.Contains("tell me more", StringComparison.OrdinalIgnoreCase) ||
            respond.Contains("elaborate", StringComparison.OrdinalIgnoreCase) ||
            respond.Contains("more details", StringComparison.OrdinalIgnoreCase))
        {
            return HandleTellMeMore();
        }

        // Check for memory recall
        if (respond.Contains("what did we talk about", StringComparison.OrdinalIgnoreCase) ||
            respond.Contains("recall", StringComparison.OrdinalIgnoreCase) ||
            respond.Contains("remember", StringComparison.OrdinalIgnoreCase))
        {
            return RecallConversation();
        }

        // Check for clear memory
        if (respond.Contains("clear memory", StringComparison.OrdinalIgnoreCase) ||
            respond.Contains("forget everything", StringComparison.OrdinalIgnoreCase))
        {
            ClearMemory();
            return "I've cleared my memory. What would you like to discuss now?";
        }

        // Store the topic from the message
        string detectedTopic = StoreTopic(respond);
        if (!string.IsNullOrEmpty(detectedTopic))
        {
            currentTopic = detectedTopic;
        }
        
        string lowerRespond = respond.ToLower();

        // Check security topics FIRST (before greetings)
        if (lowerRespond.Contains("password"))
            return Definitions(respond);
        
        if (lowerRespond.Contains("phishing"))
            return Definitions(respond);
        
        if (lowerRespond.Contains("safe browsing") || lowerRespond.Contains("safe") && lowerRespond.Contains("browsing"))
            return Definitions(respond);
        
        if (lowerRespond.Contains("browsing") && !lowerRespond.Contains("safe"))
            return Definitions(respond);
        
        // Check goodbye
        if (lowerRespond.Contains("bye") || lowerRespond.Contains("goodbye") || lowerRespond.Contains("see you"))
            return "Goodbye! Enjoy the rest of your day! Stay safe online! 👋";
        
        // Check thanks
        if (lowerRespond.Contains("thanks") || lowerRespond.Contains("thank you") || lowerRespond.Contains("appreciate"))
            return "You're welcome! It's my pleasure to help you with cybersecurity. 😊";
        
        // Check questions about bot capabilities
        if (lowerRespond.Contains("what can i ask") || lowerRespond.Contains("ask about you") || 
            lowerRespond.Contains("can i ask") || lowerRespond.Contains("what do you know"))
        {
            return "I specialize in **Cybersecurity topics** including:\n" +
                   "• 🔐 Passwords - creation, management, and security\n" +
                   "• 🎣 Phishing - how to identify and avoid attacks\n" +
                   "• 🌐 Safe Browsing - secure internet habits\n\n" +
                   "You can ask me: 'What is phishing?', 'Tell me about passwords', or 'Safe browsing tips'";
        }
        
        // Check responses to "how are you"
        if (lowerRespond.Contains("am good") || lowerRespond.Contains("i'm good") || 
            lowerRespond.Contains("yourself") || lowerRespond.Contains("you?"))
        {
            return "I'm also good too! Glad to hear that! 😊 Ready to learn about cybersecurity?";
        }
        
        // Check "how are you" question
        if (lowerRespond.Contains("how are you") || lowerRespond.Contains("how's it going"))
        {
            return "I'm functioning perfectly! Thanks for asking! How can I help you with cybersecurity today?";
        }
        
        // Check greetings (LAST - after all other checks)
        if (lowerRespond.Contains("hello") || lowerRespond.Contains("hey") || lowerRespond.Contains("hy"))
        {
            return GetPersonalizedGreeting();
        }
        
        if (lowerRespond.Contains("hi"))
        {
            return GetPersonalizedGreeting();
        }
        
        // Default response with context awareness
        if (!string.IsNullOrEmpty(currentTopic))
        {
            return $"I noticed you were asking about {currentTopic}. Would you like me to tell you more about it? Or ask me about passwords, phishing, or safe browsing!";
        }
        
        return "Oops! I didn't understand that. 🤔 Try asking about:\n" +
               "• 'What is password?'\n" +
               "• 'Explain phishing'\n" +
               "• 'Safe browsing tips'\n" +
               "• 'Tell me more' (after asking about a topic)";
    }

    private string GetPersonalizedGreeting()
    {
        if (!string.IsNullOrEmpty(currentTopic))
        {
            return $"Welcome back! 👋 Last time we were discussing **{currentTopic}**. Would you like to continue or explore something new?";
        }
        return "Hello! Welcome to CyberBot! 🛡️ I can teach you about:\n" +
               "• Password security\n" +
               "• Phishing attacks\n" +
               "• Safe browsing habits\n\n" +
               "What would you like to learn about?";
    }

    private string HandleTellMeMore()
    {
        if (!string.IsNullOrEmpty(currentTopic))
        {
            string detailedResponse = GetDetailedResponse(currentTopic);
            // Store the detailed response in memory
            if (!topicDetails.ContainsKey(currentTopic))
                topicDetails[currentTopic] = new List<string>();
            
            if (!topicDetails[currentTopic].Contains(detailedResponse))
                topicDetails[currentTopic].Add(detailedResponse);
            
            return detailedResponse;
        }
        else
        {
            return "You haven't asked about any security topics yet. Try asking me:\n" +
                   "• 'What is a password?'\n" +
                   "• 'Explain phishing'\n" +
                   "• 'Tell me about safe browsing'\n\n" +
                   "Then say 'tell me more' for detailed information!";
        }
    }

    private string RecallConversation()
    {
        if (conversationHistory.Count == 0)
            return "We haven't talked about anything yet! Ask me something about cybersecurity first.";
        
        string history = "📝 **Here's what we discussed recently:**\n";
        for (int i = Math.Max(0, conversationHistory.Count - 5); i < conversationHistory.Count; i++)
        {
            history += $"• {conversationHistory[i]}\n";
        }
        
        if (!string.IsNullOrEmpty(currentTopic))
            history += $"\n💡 The main topic we're discussing is: **{currentTopic}**";
        
        return history;
    }
    
    public string StoreTopic(string message)
    {
        string[] keywords = { "password", "security", "phishing", "safe browsing", "browsing" };

        foreach (string keyword in keywords)
        {
            if (message.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            {
                currentTopic = keyword;
                return keyword;
            }
        }
        return null;
    }

    private string GetDetailedResponse(string keyword)
    {
        switch (keyword.ToLower())
        {
            case "password":
                return "🔐 **Password Security Tips:**\n" +
                       "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                       "✓ Use 12+ characters with uppercase, lowercase, numbers, and symbols\n" +
                       "✓ Never reuse passwords across different sites\n" +
                       "✓ Use a password manager like Bitwarden or LastPass\n" +
                       "✓ Enable 2-factor authentication (2FA) whenever possible\n" +
                       "✓ Change passwords every 3-6 months for critical accounts\n" +
                       "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n\n" +
                       "💡 **Pro Tip:** A passphrase like 'Blue-Horse-Star-Coffee!' is stronger than 'P@ssw0rd123'";

            case "security":
                return "🛡️ **Security Best Practices:**\n" +
                       "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                       "✓ Keep all software and apps updated\n" +
                       "✓ Use a VPN on public Wi-Fi\n" +
                       "✓ Enable firewall protection\n" +
                       "✓ Be cautious of suspicious links and attachments\n" +
                       "✓ Regularly backup important data\n" +
                       "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n\n" +
                       "💡 **Remember:** Security is a process, not a product!";

            case "phishing":
                return "🎣 **How to Spot Phishing Attacks:**\n" +
                       "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                       "✓ Check sender's email address carefully (spoofing is common)\n" +
                       "✓ Hover over links before clicking to see the real URL\n" +
                       "✓ Look for poor grammar and spelling errors\n" +
                       "✓ Never share passwords or sensitive info via email\n" +
                       "✓ When in doubt, contact the company directly\n" +
                       "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n\n" +
                       "📧 **Example:** 'Urgent: Your account will be closed!' is often a phishing scam!";

            case "safe browsing":
            case "browsing":
                return "🌐 **Safe Browsing Tips:**\n" +
                       "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                       "✓ Only visit trusted websites (look for HTTPS 🔒)\n" +
                       "✓ Avoid downloading files from unknown sources\n" +
                       "✓ Keep your browser and extensions updated\n" +
                       "✓ Use ad-blockers to avoid malicious ads\n" +
                       "✓ Clear your browser cache and cookies regularly\n" +
                       "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n\n" +
                       "🛒 **Online Shopping:** Always check for 'https://' and the padlock icon before entering payment info!";

            default:
                return $"I can provide more details about **{keyword}**. What specific aspect would you like to know?";
        }
    }

    public string Definitions(string respond)
    {
        string lowerInput = respond.ToLower();

        if (lowerInput.Contains("password"))
        {
            return $"🔑 **Password Definition:** {shortDefinitions["password"]}\n\n" +
                   "**Strong Password Tips:**\n" +
                   "• Use at least 8-12 characters\n" +
                   "• Include uppercase, lowercase, numbers, and symbols\n" +
                   "• Avoid personal info like your name or birthdate\n" +
                   "• Never reuse passwords across different sites\n\n" +
                   "💡 **Example:** 'MyDogMax123!' is weak → 'M!D0gm@x2024!' is stronger\n\n" +
                   "➡️ **Say 'tell me more' for detailed password security tips!**";
        }
        
        if (lowerInput.Contains("phishing"))
        {
            return $"🎣 **Phishing Definition:** {shortDefinitions["phishing"]}\n\n" +
                   "**How to Protect Yourself:**\n" +
                   "• Always check the sender's email address\n" +
                   "• Hover over links before clicking\n" +
                   "• Never share passwords or OTPs with anyone\n" +
                   "• Look for urgency tactics ('Your account will be closed!')\n\n" +
                   "📧 **Example Phishing Email:** 'Dear User, verify your account now or it will be deleted!'\n\n" +
                   "➡️ **Say 'tell me more' for detailed phishing prevention tips!**";
        }
        
        if (lowerInput.Contains("safe") || lowerInput.Contains("browsing"))
        {
            return $"🌐 **Safe Browsing Definition:** {shortDefinitions["safe browsing"]}\n\n" +
                   "**Safe Browsing Tips:**\n" +
                   "• Visit only trusted websites with HTTPS (🔒)\n" +
                   "• Avoid suspicious downloads and pop-ups\n" +
                   "• Keep your browser updated\n" +
                   "• Use antivirus and firewall protection\n\n" +
                   "🔒 **Quick Check:** Look for the padlock icon in your browser's address bar!\n\n" +
                   "➡️ **Say 'tell me more' for detailed safe browsing tips!**";
        }
        
        return "I'm sorry, I didn't understand that. 🤔 Please ask about:\n" +
               "• 'password' - Learn about password security\n" +
               "• 'phishing' - Understand phishing attacks\n" +
               "• 'safe browsing' - Get internet safety tips";
    }
    
    public void ClearMemory()
    {
        currentTopic = null;
        topicDetails.Clear();
        conversationHistory.Clear();
    }
    
    public string GetCurrentTopic()
    {
        return currentTopic ?? "No topic currently being discussed. Try asking about passwords, phishing, or safe browsing!";
    }
    
    public string GetConversationStats()
    {
        return $"📊 **Stats:** Discussed {conversationHistory.Count} messages. Current topic: {GetCurrentTopic()}";
    }
}