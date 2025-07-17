export function bytesToImageUrl(avatar?: number[]) {
  if (!avatar || !avatar.length) return undefined;
  const byteArray = new Uint8Array(avatar);
  const blob = new Blob([byteArray], { type: "image/png" });
  return URL.createObjectURL(blob);
}
