public static class StringExtensions
{
    /// <summary>
    /// Fowler–Noll–Vo (or FNV) is a non-cryptographic hash function
    /// Start with an initial hash value of FNV offset basis. For each byte in the input, 
    /// multiply hash by the FNV prime, then XOR it with the byte from the input. 
    /// The alternate algorithm, FNV-1a, reverses the multiply and XOR steps.
    /// Very good at producing unique keys
    /// </summary>
    /// <param name="s">string</param>
    /// <returns>FNV-1a hash</returns>
    public static int ComputeHash(this string s)
    {
        uint hash = 2166136261; // just a long prime number
        foreach (char c in s)
        {
            hash = (hash ^ c) * 16777619;
        }
        return unchecked((int)hash);
    }
}