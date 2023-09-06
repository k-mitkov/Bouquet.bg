export function base64UrlDecode(input: string): string {
    // Replacing URL-safe characters with base64 characters
    const base64 = input.replace(/-/g, '+').replace(/_/g, '/');
  
    // Padding the string if necessary
    const paddedBase64 = base64.padEnd(base64.length + (4 - (base64.length % 4)) % 4, '=');
  
    // Decoding the base64 string
    const decoded = atob(paddedBase64);
  
    // Decoding the byte array into a string using UTF-8
    const utf8Decoder = new TextDecoder('utf-8');
    return utf8Decoder.decode(new Uint8Array(decoded.split('').map((char) => char.charCodeAt(0))));
  }