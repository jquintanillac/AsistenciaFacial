export async function detectFace(videoElement, canvasElement) {
    if (!videoElement || videoElement.paused || videoElement.ended) return null;

    const faceApi = window.faceapi;
    if (!faceApi) return null;

    // Use TinyFaceDetector for performance
    const options = new faceApi.TinyFaceDetectorOptions();
    
    const detection = await faceApi.detectSingleFace(videoElement, options)
        .withFaceLandmarks()
        .withFaceDescriptor();

    if (detection) {
        // Resize detection to match canvas size
        const width = videoElement.clientWidth;
        const height = videoElement.clientHeight;

        if (width === 0 || height === 0) return null;

        const displaySize = { width: width, height: height };
        faceApi.matchDimensions(canvasElement, displaySize);
        
        const resizedDetections = faceApi.resizeResults(detection, displaySize);
        
        // Draw detections
        const context = canvasElement.getContext('2d');
        context.clearRect(0, 0, canvasElement.width, canvasElement.height);
        faceApi.draw.drawDetections(canvasElement, resizedDetections);
        faceApi.draw.drawFaceLandmarks(canvasElement, resizedDetections);
        
        return detection.descriptor;
    } else {
        const context = canvasElement.getContext('2d');
        context.clearRect(0, 0, canvasElement.width, canvasElement.height);
    }
    
    return null;
}
