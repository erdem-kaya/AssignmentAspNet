// Real-Time form validation och after submit error handling
// Chatgpt hjälpte mig med att skriva denna kod
const validateField = (field) => {
    let errorSpan = document.querySelector(`span[data-valmsg-for='${field.name}']`);
    if (!errorSpan) return;

    let errorMessage = "";
    let value = field.value.trim();

    // [Required]
    if (field.hasAttribute("data-val-required") && value === "")
        errorMessage = field.getAttribute("data-val-required");

    // [Regex]
    if (field.hasAttribute("data-val-regex") && value !== "") {
        let pattern = new RegExp(field.getAttribute("data-val-regex-pattern"));
        if (!pattern.test(value))
            errorMessage = field.getAttribute("data-val-regex");
    }

    // [Compare]
    if (field.hasAttribute("data-val-equalto") && value !== "") {
        let otherField = document.querySelector(`[name="${field.getAttribute("data-val-equalto-other").replace("*.", "")}"]`);
        if (otherField && otherField.value !== value)
            errorMessage = field.getAttribute("data-val-equalto");
    }

    if (errorMessage) {
        field.classList.add("input-validation-error");
        errorSpan.classList.remove("field-validation-valid");
        errorSpan.classList.add("field-validation-error");
        errorSpan.textContent = errorMessage;
    } else {
        field.classList.remove("input-validation-error");
        errorSpan.classList.remove("field-validation-error");
        errorSpan.classList.add("field-validation-valid");
        errorSpan.textContent = "";
    }
};

const clearErrorMessages = (form) => {
    form.querySelectorAll('[data-val="true"]').forEach(input => {
        input.classList.remove('input-validation-error');
    });

    form.querySelectorAll('[data-valmsg-for]').forEach(span => {
        span.innerText = '';
        span.classList.remove('field-validation-error');
    });

    const alertBox = document.querySelector('.alert-notification');
    if (alertBox) {
        alertBox.innerText = '';
        alertBox.classList.remove('error');
        alertBox.style.display = 'none';
    }
};

document.addEventListener("DOMContentLoaded", function () {

    const forms = document.querySelectorAll("form");

    forms.forEach(form => {
        const fields = form.querySelectorAll("input[data-val='true']");
        fields.forEach(field => {
            field.addEventListener("input", () => validateField(field));
        });

        form.addEventListener("submit", async (e) => {
            e.preventDefault();
            clearErrorMessages(form);

            let allValid = true;
            fields.forEach(field => {
                validateField(field);
                if (field.classList.contains("input-validation-error"))
                    allValid = false;
            });

            if (!allValid)
                return;

            const formData = new FormData(form);

            try {
                const res = await fetch(form.action, {
                    method: form.method,
                    body: formData
                });

                if (res.status === 400) {
                    const data = await res.json();

                    if (data.globalError) {
                        const alertBox = document.querySelector('.alert-notification');
                        if (alertBox) {
                            alertBox.innerText = data.globalError;
                            alertBox.classList.add('error');
                            alertBox.style.display = 'block';
                        }
                    }

                    if (data.errors) {
                        Object.keys(data.errors).forEach(key => {
                            const input = form.querySelector(`[name="${key}"]`);
                            if (input)
                                input.classList.add("input-validation-error");

                            const span = form.querySelector(`[data-valmsg-for="${key}"]`);
                            if (span) {
                                span.textContent = data.errors[key][0];
                                span.classList.add("field-validation-error");
                            }
                        });
                    }
                }
                else if (res.redirected) {
                    window.location.href = res.url;
                }
            } catch (error) {
                console.error("Form submit error:", error);
            }
        });
    });
});
