using OtpNet;

namespace OTP.Domain
{
    public sealed class OtpNETGenerator : IOTPGenerator
    {
        private readonly Totp totp;

        public OtpNETGenerator(Totp totp)
        {
            this.totp = totp;
        }

        public OneTimePassword GenerateOTP(NonEmptyString userId, DateTimeOffset momentOfRequest)
        {
            var otp = totp.ComputeTotp(momentOfRequest.DateTime);

            return new OneTimePassword(otp);
        }
    }
}
