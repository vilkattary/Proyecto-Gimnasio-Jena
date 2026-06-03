function toggleSwitch() {
    var cb = document.getElementById('estadoClaseToggle');
    cb.checked = !cb.checked;
    updateToggleUI(cb.checked);
}

function updateToggleUI(isChecked) {
    var track = document.getElementById('toggleTrack');
    var thumb = document.getElementById('toggleThumb');
    var label = document.getElementById('estadoLabel');

    if (isChecked) {
        track.style.backgroundColor = 'var(--jena-orange)';
        thumb.style.transform = 'translateX(28px)';
        label.textContent = 'Activa';
        label.style.color = 'var(--jena-orange)';
    } else {
        track.style.backgroundColor = '#ddd';
        thumb.style.transform = 'translateX(0)';
        label.textContent = 'Inactiva';
        label.style.color = 'var(--jena-light-blue)';
    }
}

// Inicializar según valor del modelo al cargar
document.addEventListener('DOMContentLoaded', function () {
    var cb = document.getElementById('estadoClaseToggle');
    updateToggleUI(cb.checked);
});