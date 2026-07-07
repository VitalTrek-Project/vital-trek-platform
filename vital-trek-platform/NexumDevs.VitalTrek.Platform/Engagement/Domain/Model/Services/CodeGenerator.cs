namespace NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Services;

/// <summary>
/// Generates short, human-friendly codes for redemptions and referral links.
/// Excludes visually ambiguous characters (0/O, 1/I) to reduce transcription errors
/// when a tourist reads or types a code manually.
/// </summary>
internal static class CodeGenerator
{
    private const string Alphabet = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";

    public static string Generate(int length = 8)
    {
        Span<char> buffer = stackalloc char[length];
        for (var i = 0; i < length; i++)
            buffer[i] = Alphabet[Random.Shared.Next(Alphabet.Length)];
        return new string(buffer);
    }
}
