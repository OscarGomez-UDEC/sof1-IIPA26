// Validación de Login en español
document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById('formLogin');
    const usuarioInput = document.getElementById('NombreUsuario');
    const passwordInput = document.getElementById('Contrasena');

    // Al enviar el formulario
    form.addEventListener('submit', function (e) {
        // Validar que no esté vacío
        if (!usuarioInput.value.trim() || !passwordInput.value.trim()) {
            e.preventDefault();
            e.stopPropagation();

            // Marcar campos inválidos
            if (!usuarioInput.value.trim()) {
                usuarioInput.classList.add('is-invalid');
            }
            if (!passwordInput.value.trim()) {
                passwordInput.classList.add('is-invalid');
            }

            form.classList.add('was-validated');
            return false;
        }

        // Si todo es válido, dejar que se envíe
        form.classList.add('was-validated');
    }, false);

    // Limpiar error al escribir
    usuarioInput.addEventListener('input', function () {
        if (this.value.trim()) {
            this.classList.remove('is-invalid');
        }
    });

    passwordInput.addEventListener('input', function () {
        if (this.value.trim()) {
            this.classList.remove('is-invalid');
        }
    });
});
