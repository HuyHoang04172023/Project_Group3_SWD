namespace Project_Group3_SWD.Constants
{
    public class EmailConstants
    {
        public const string GMAIL_REGISTER_EMAIL_SUBJECT = "Welcome to SHOPQUANAO!";

        public const string GMAIL_REGISTER_EMAIL_BODY = @"
            Welcome to SHOPQUANAO! Your account has been successfully created via Google Sign-In. Here are your account details:

            Registered Email: {1}  

            Use the link below to set a password for your account:

            🔐 [Set Your Password]({2})

            If this wasn’t you, please contact our support team immediately.

            Best regards,  
            SHOPQUANAO Team  

            support@SHOPQUANAO.com | SHOPQUANAO";
    }
}
