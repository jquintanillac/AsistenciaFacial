import { loadModels } from './face-loader.js';
import { startCamera, stopCamera } from './face-capture.js';
import { detectFace } from './face-recognition.js';

let detectionInterval = null;
let dotNetRef = null;

export async function initFaceService(dotNetReference, modelPath) {
    dotNetRef = dotNetReference;
    await loadModels(modelPath);
}

export async function startVideo(videoId, canvasId) {
    const video = document.getElementById(videoId);
    const canvas = document.getElementById(canvasId);
    
    if (!video || !canvas) {
        console.error("Video or canvas element not found");
        return;
    }

    try {
        await startCamera(video);
        
        video.onplay = () => {
            if (video.clientWidth === 0 || video.clientHeight === 0) {
                console.warn("Video dimensions are 0, waiting for metadata...");
                return; // Wait for next event or retry
            }

            const displaySize = { width: video.clientWidth, height: video.clientHeight };
            if (window.faceapi) {
                window.faceapi.matchDimensions(canvas, displaySize);
            }

            if (detectionInterval) clearInterval(detectionInterval);

            detectionInterval = setInterval(async () => {
                const descriptor = await detectFace(video, canvas);
                if (descriptor) {
                    // Notify Blazor that face is detected
                    // Convert Float32Array to normal array
                    if (dotNetRef) {
                        dotNetRef.invokeMethodAsync('OnFaceDetected', Array.from(descriptor));
                    }
                } else {
                    if (dotNetRef) {
                        dotNetRef.invokeMethodAsync('OnFaceLost');
                    }
                }
            }, 200); // Check every 200ms
        };
    } catch (error) {
        console.error("Error starting video:", error);
        throw error;
    }
}

export function stopVideo(videoId) {
    const video = document.getElementById(videoId);
    if (detectionInterval) clearInterval(detectionInterval);
    stopCamera(video);
}

export function captureImage(videoId) {
    const video = document.getElementById(videoId);
    if (!video) return null;
    
    const canvas = document.createElement('canvas');
    canvas.width = video.videoWidth;
    canvas.height = video.videoHeight;
    canvas.getContext('2d').drawImage(video, 0, 0);
    return canvas.toDataURL('image/jpeg');
}
