// Place this in your wwwroot/js/alert-interop.js file

window.alertInterop = {
    dotNetReference: null,

    initialize: function (dotNetRef) {
        this.dotNetReference = dotNetRef;
    },

    showSuccessAlert: function () {
        const alertElement = document.getElementById('loginSuccessAlert');
        if (alertElement) {
            // Add the 'show' class to trigger the Bootstrap fade in
            alertElement.classList.add('show');
            alertElement.style.display = 'block';

            // Auto-dismiss after 3 seconds
            setTimeout(() => {
                this.hideSuccessAlert();
            }, 3000);
        }
    },

    hideSuccessAlert: function () {
        const alertElement = document.getElementById('loginSuccessAlert');
        if (alertElement) {
            alertElement.classList.remove('show');
            // Wait for the fade out transition
            setTimeout(() => {
                alertElement.style.display = 'none';
            }, 150);
        }
    }
};