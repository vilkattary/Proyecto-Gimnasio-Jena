function aplicarFiltros() {
    var tipo = document.getElementById('filtroTipo').value.toLowerCase();
    var estado = document.getElementById('filtroEstado').value.toLowerCase();
    var orden = document.getElementById('filtroOrden').value;
    var items = Array.from(document.querySelectorAll('.clase-item'));

  
    items.forEach(function (item) {
        var matchTipo = !tipo || item.dataset.tipo.includes(tipo);
        var matchEstado = !estado || item.dataset.estado === estado;
        item.style.display = (matchTipo && matchEstado) ? '' : 'none';
    });

   
    if (orden) {
        var contenedor = document.getElementById('listaClases');
        var visibles = items.filter(function (i) { return i.style.display !== 'none'; });

        visibles.sort(function (a, b) {
            if (orden === 'instructor') return a.dataset.instructor.localeCompare(b.dataset.instructor);
            if (orden === 'cupos') return parseInt(b.dataset.cupos) - parseInt(a.dataset.cupos);
            return 0;
        });

        visibles.forEach(function (item) { contenedor.appendChild(item); });
    }
}
