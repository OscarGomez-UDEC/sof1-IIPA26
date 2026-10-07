// Validación de formulario de registro en español
document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById('formRegistro');
    if (!form) return;

    const nameInput = document.getElementById('Name');
    const usernameInput = document.getElementById('Username');
    const passwordInput = document.getElementById('Password');
    const confirmPasswordInput = document.getElementById('ConfirmPassword');

    // Validación en tiempo real - Nombre
    nameInput.addEventListener('input', function () {
        if (this.value.trim() !== '') {
            this.classList.remove('is-invalid');
        }
    });

    // Validación en tiempo real - Username
    usernameInput.addEventListener('input', function () {
        if (this.value.trim().length >= 4) {
            this.classList.remove('is-invalid');
        }
    });

    // Validación en tiempo real - Password
    passwordInput.addEventListener('input', function () {
        if (this.value.length >= 6) {
            this.classList.remove('is-invalid');
        }
        // Si cambió la contraseña, revalidar confirmación
        validarCoincidenciaContraseñas();
    });

    // Validación en tiempo real - Confirm Password
    confirmPasswordInput.addEventListener('input', function () {
        validarCoincidenciaContraseñas();
    });

    // Al enviar el formulario
    form.addEventListener('submit', function (e) {
        // Validar contraseña
        if (!validarFortalezaContraseña()) {
            e.preventDefault();
            e.stopPropagation();
            return false;
        }

        // Validar que coincidan
        if (!validarCoincidenciaContraseñas()) {
            e.preventDefault();
            e.stopPropagation();
            return false;
        }

        // Validaciones nativas de HTML5
        if (!form.checkValidity()) {
            e.preventDefault();
            e.stopPropagation();
        }

        form.classList.add('was-validated');
    }, false);
});

function validarCoincidenciaContraseñas() {
    const password = document.getElementById('Password').value;
    const confirmPassword = document.getElementById('ConfirmPassword').value;
    const confirmField = document.getElementById('ConfirmPassword');

    if (password && confirmPassword) {
        if (password !== confirmPassword) {
            confirmField.classList.add('is-invalid');
            return false;
        } else {
            confirmField.classList.remove('is-invalid');
            return true;
        }
    } else if (!confirmPassword) {
        // Si el campo está vacío, no mostrar error
        confirmField.classList.remove('is-invalid');
    }
    return true;
}

function validarFortalezaContraseña() {
    const password = document.getElementById('Password').value;
    const passwordField = document.getElementById('Password');

    if (password) {
        if (password.length < 6) {
            passwordField.classList.add('is-invalid');
            return false;
        } else {
            passwordField.classList.remove('is-invalid');
            return true;
        }
    }
    return true;
}
