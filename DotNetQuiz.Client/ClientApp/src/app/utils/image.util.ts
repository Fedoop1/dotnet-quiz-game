export function byteArrayToBase64(buffer: number[]) {
  let result = '';
  const bytes = new Uint8Array(buffer);
  for (let index = 0; index < bytes.length; index++) {
    result += String.fromCharCode(bytes[index]);
  }

  return window.btoa(result);
}
