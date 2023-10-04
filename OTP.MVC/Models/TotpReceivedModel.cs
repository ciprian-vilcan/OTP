using OTP.Domain;

namespace OTP.MVC.Models;

public record TotpReceivedModel(NonEmptyString UserId, Totp Totp);
