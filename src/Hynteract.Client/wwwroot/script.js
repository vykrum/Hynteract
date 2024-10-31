let cube = document.getElementById('cube');
let isDragging = false;
let previousMousePosition = { x: 0, y: 0 };
let rotation = { x: 30, y: 45 }; // Adjusted for axonometric view

document.addEventListener('mousedown', function (e) {
    isDragging = true;
    previousMousePosition = { x: e.clientX, y: e.clientY };
});

document.addEventListener('mousemove', function (e) {
    if (isDragging) {
        let deltaX = e.clientX - previousMousePosition.x;
        let deltaY = e.clientY - previousMousePosition.y;
        rotation.y += deltaX * 0.5;
        rotation.x -= deltaY * 0.5;
        cube.style.transform = `rotateX(${rotation.x}deg) rotateY(${rotation.y}deg)`;
        previousMousePosition = { x: e.clientX, y: e.clientY };
    }
});

document.addEventListener('mouseup', function () {
    isDragging = false;
});

document.addEventListener('touchstart', function (e) {
    isDragging = true;
    previousMousePosition = { x: e.touches[0].clientX, y: e.touches[0].clientY };
});

document.addEventListener('touchmove', function (e) {
    if (isDragging) {
        let deltaX = e.touches[0].clientX - previousMousePosition.x;
        let deltaY = e.touches[0].clientY - previousMousePosition.y;
        rotation.y += deltaX * 0.5;
        rotation.x -= deltaY * 0.5;
        cube.style.transform = `rotateX(${rotation.x}deg) rotateY(${rotation.y}deg)`;
        previousMousePosition = { x: e.touches[0].clientX, y: e.touches[0].clientY };
    }
});

document.addEventListener('touchend', function () {
    isDragging = false;
});

// Render the template
document.body.appendChild(document.getElementById('Draw').content);
