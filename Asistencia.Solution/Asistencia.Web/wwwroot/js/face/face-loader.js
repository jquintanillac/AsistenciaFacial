export async function loadModels(modelPath) {
    console.log("Loading Face API models from", modelPath);
    
    const faceApi = window.faceapi;
    if (!faceApi) {
        throw new Error("face-api.js not loaded. Check index.html");
    }

    try {
        await faceApi.nets.tinyFaceDetector.loadFromUri(modelPath);
        await faceApi.nets.faceLandmark68Net.loadFromUri(modelPath);
        await faceApi.nets.faceRecognitionNet.loadFromUri(modelPath);
        console.log("Models loaded successfully.");
    } catch (error) {
        console.error("Error loading models:", error);
        throw error;
    }
}
