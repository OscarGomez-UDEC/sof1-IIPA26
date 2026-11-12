// Variables globales
let usuariosFiltrados = [];
let usuariosCompletos = [];
let currentPage = 1;
let recordsPerPage = 5;

document.addEventListener('DOMContentLoaded', function () {
    // Inicializar datos
    usuariosCompletos = usuariosData || [];
    usuariosFiltrados = [...usuariosCompletos];

    // Event listeners
    document.getElementById('recordsPerPage').addEventListener('change', function () {
        recordsPerPage = parseInt(this.value);
        currentPage = 1;
        renderTable();
    });

    // Filtros
    document.getElementById('filterNombre').addEventListener('keyup', aplicarFiltros);
    document.getElementById('filterUsuario').addEventListener('keyup', aplicarFiltros);
    document.getElementById('filterRol').addEventListener('keyup', aplicarFiltros);
    document.getElementById('filterEstado').addEventListener('keyup', aplicarFiltros);

    // Botón limpiar filtros
    document.getElementById('btnLimpiarFiltros').addEventListener('click', limpiarFiltros);

    // Renderizar tabla inicial
    renderTable();
});

function aplicarFiltros() {
    const filterNombre = document.getElementById('filterNombre').value.toLowerCase();
    const filterUsuario = document.getElementById('filterUsuario').value.toLowerCase();
    const filterRol = document.getElementById('filterRol').value.toLowerCase();
    const filterEstado = document.getElementById('filterEstado').value.toLowerCase();

    usuariosFiltrados = usuariosCompletos.filter(usuario => {
        const nombre = usuario.name.toLowerCase();
        const username = usuario.username.toLowerCase();
        const role = usuario.role.toLowerCase();
        const state = usuario.state.toLowerCase();

        return nombre.includes(filterNombre) &&
            username.includes(filterUsuario) &&
            role.includes(filterRol) &&
            state.includes(filterEstado);
    });

    currentPage = 1;
    renderTable();
}

function limpiarFiltros() {
    document.getElementById('filterNombre').value = '';
    document.getElementById('filterUsuario').value = '';
    document.getElementById('filterRol').value = '';
    document.getElementById('filterEstado').value = '';

    usuariosFiltrados = [...usuariosCompletos];
    currentPage = 1;
    renderTable();
}

function renderTable() {
    const tbody = document.getElementById('usuariosTableBody');
    tbody.innerHTML = '';

    const startIdx = (currentPage - 1) * recordsPerPage;
    const endIdx = startIdx + recordsPerPage;
    const usuariosParaMostrar = usuariosFiltrados.slice(startIdx, endIdx);

    if (usuariosParaMostrar.length === 0) {
        tbody.innerHTML = '<tr><td colspan="6" class="text-center text-muted">No hay registros que coincidan con los filtros</td></tr>';
        renderPagination();
        updatePaginationInfo(0, 0);
        return;
    }

    usuariosParaMostrar.forEach(usuario => {
        const row = document.createElement('tr');

        // Determinar color del badge de estado
        const badgeEstado = usuario.state === 'Activo' 
            ? `<span class="badge bg-success">${usuario.state}</span>`
            : `<span class="badge bg-danger">${usuario.state}</span>`;

        row.innerHTML = `
            <td><span class="badge bg-secondary">${usuario.idUser}</span></td>
            <td>${usuario.name}</td>
            <td><code>${usuario.username}</code></td>
            <td><span class="badge bg-info">${usuario.role}</span></td>
            <td>${badgeEstado}</td>
            <td>
                <button class="btn btn-sm btn-warning" data-bs-toggle="tooltip" title="Editar">
                    <i class="bi bi-pencil"></i>
                </button>
                <button class="btn btn-sm btn-danger" data-bs-toggle="tooltip" title="Eliminar">
                    <i class="bi bi-trash"></i>
                </button>
            </td>
        `;
        tbody.appendChild(row);
    });

    renderPagination();
    updatePaginationInfo(startIdx + 1, Math.min(endIdx, usuariosFiltrados.length));

    // Reinicializar tooltips
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.forEach(function (tooltipTriggerEl) {
        new bootstrap.Tooltip(tooltipTriggerEl);
    });
}

function renderPagination() {
    const totalPages = Math.ceil(usuariosFiltrados.length / recordsPerPage);
    const paginationContainer = document.getElementById('pagination');

    if (totalPages <= 1) {
        paginationContainer.style.display = 'none';
        return;
    }

    paginationContainer.style.display = 'flex';
    paginationContainer.innerHTML = '';

    // Botón anterior
    const prevLi = document.createElement('li');
    prevLi.className = `page-item ${currentPage === 1 ? 'disabled' : ''}`;
    prevLi.innerHTML = '<a class="page-link" href="#" tabindex="-1">Anterior</a>';
    prevLi.addEventListener('click', function (e) {
        e.preventDefault();
        if (currentPage > 1) {
            currentPage--;
            renderTable();
        }
    });
    paginationContainer.appendChild(prevLi);

    // Números de página
    let startPage = Math.max(1, currentPage - 2);
    let endPage = Math.min(totalPages, currentPage + 2);

    if (startPage > 1) {
        const firstLi = document.createElement('li');
        firstLi.className = 'page-item';
        firstLi.innerHTML = '<a class="page-link" href="#">1</a>';
        firstLi.addEventListener('click', function (e) {
            e.preventDefault();
            currentPage = 1;
            renderTable();
        });
        paginationContainer.appendChild(firstLi);

        if (startPage > 2) {
            const dotsLi = document.createElement('li');
            dotsLi.className = 'page-item disabled';
            dotsLi.innerHTML = '<span class="page-link">...</span>';
            paginationContainer.appendChild(dotsLi);
        }
    }

    for (let i = startPage; i <= endPage; i++) {
        const li = document.createElement('li');
        li.className = `page-item ${i === currentPage ? 'active' : ''}`;
        li.innerHTML = `<a class="page-link" href="#">${i}</a>`;
        li.addEventListener('click', function (e) {
            e.preventDefault();
            currentPage = i;
            renderTable();
        });
        paginationContainer.appendChild(li);
    }

    if (endPage < totalPages) {
        if (endPage < totalPages - 1) {
            const dotsLi = document.createElement('li');
            dotsLi.className = 'page-item disabled';
            dotsLi.innerHTML = '<span class="page-link">...</span>';
            paginationContainer.appendChild(dotsLi);
        }

        const lastLi = document.createElement('li');
        lastLi.className = 'page-item';
        lastLi.innerHTML = `<a class="page-link" href="#">${totalPages}</a>`;
        lastLi.addEventListener('click', function (e) {
            e.preventDefault();
            currentPage = totalPages;
            renderTable();
        });
        paginationContainer.appendChild(lastLi);
    }

    // Botón siguiente
    const nextLi = document.createElement('li');
    nextLi.className = `page-item ${currentPage === totalPages ? 'disabled' : ''}`;
    nextLi.innerHTML = '<a class="page-link" href="#">Siguiente</a>';
    nextLi.addEventListener('click', function (e) {
        e.preventDefault();
        if (currentPage < totalPages) {
            currentPage++;
            renderTable();
        }
    });
    paginationContainer.appendChild(nextLi);
}

function updatePaginationInfo(start, end) {
    document.getElementById('startRecord').textContent = usuariosFiltrados.length > 0 ? start : 0;
    document.getElementById('endRecord').textContent = end;
    document.getElementById('totalRecords').textContent = usuariosFiltrados.length;
    document.getElementById('resultadosCount').innerHTML = `Total: <strong>${usuariosFiltrados.length}</strong>`;
}
