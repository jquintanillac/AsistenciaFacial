export async function startCamera(videoElement) {
    try {
        const stream = await navigator.mediaDevices.getUserMedia({ video: {} });
        videoElement.srcObject = stream;
        return stream;
    } catch (err) {
        console.error("Error accessing camera:", err);
        throw err;
    }
}

export function stopCamera(videoElement) {
    if (videoElement && videoElement.srcObject) {
        const tracks = videoElement.srcObject.getTracks();
        tracks.forEach(track => track.stop());
        videoElement.srcObject = null;
    }
}
