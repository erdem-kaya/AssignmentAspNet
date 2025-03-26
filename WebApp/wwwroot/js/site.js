document.addEventListener('DOMContentLoaded', () => {

    const forms = document.querySelectorAll('form');
    forms.forEach(form => {
        form.addEventListener('submit', async (e) => {
            e.preventDefault();
            clearErrorMessages(form);

            const formData = new FormData(form);

            try {
                const res = await fetch(form.action, {
                    method: form.method,
                    body: formData
                });

                // ❌ Validation hataları varsa:
                if (res.status === 400) {
                    const data = await res.json();

                    if (data.errors) {
                        Object.keys(data.errors).forEach(key => {
                            const input = form.querySelector(`[name="${key}"]`);
                            if (input) {
                                input.classList.add('input-validation-error');
                            }

                            const span = form.querySelector(`[data-valmsg-for="${key}"]`);
                            if (span) {
                                span.innerText = data.errors[key][0];
                                span.classList.add('field-validation-error');
                            }
                        });
                    }
                }

                // ✅ Eğer yönlendirme varsa, yönlendir
                else if (res.redirected) {
                    window.location.href = res.url;
                }

            } catch (error) {
                console.error('Form submit hatası:', error);
            }
        });
    });
});

function clearErrorMessages(form) {
    form.querySelectorAll('[data-val="true"]').forEach(input => {
        input.classList.remove('input-validation-error');
    });

    form.querySelectorAll('[data-valmsg-for]').forEach(span => {
        span.innerText = '';
        span.classList.remove('field-validation-error');
    });
}
