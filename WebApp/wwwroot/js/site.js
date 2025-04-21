document.addEventListener("DOMContentLoaded", () => {
    updateRelativeTimes();
    setInterval(updateRelativeTimes, 60000);
});

// === ChatGPT helped me write this code ===
const previewSize = 150;
//  VALIDATION
const validateField = (field, form) => {
    const errorSpan = form.querySelector(`span[data-valmsg-for='${field.name}']`);
    if (!errorSpan) return;

    let errorMessage = "";
    const value = field.value.trim();

    if (field.hasAttribute("data-val-required") && value === "")
        errorMessage = field.getAttribute("data-val-required");

    if (field.hasAttribute("data-val-regex") && value !== "") {
        const pattern = new RegExp(field.getAttribute("data-val-regex-pattern"));
        if (!pattern.test(value))
            errorMessage = field.getAttribute("data-val-regex");
    }

    if (field.hasAttribute("data-val-equalto") && value !== "") {
        const otherField = form.querySelector(`[name="${field.getAttribute("data-val-equalto-other").replace("*.", "")}"]`);
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

// ATTACH FORM VALIDATION
function attachFormValidation(form) {
    if (!form) return;

    const fields = form.querySelectorAll("input[data-val='true']");
    fields.forEach(field => {
        field.addEventListener("input", () => validateField(field, form));
    });

    form.addEventListener("submit", async (e) => {
        if (form.classList.contains("external-account")) return;
        e.preventDefault();
        clearErrorMessages(form);

        let allValid = true;
        fields.forEach(field => {
            validateField(field, form);
            if (field.classList.contains("input-validation-error"))
                allValid = false;
        });

        if (!allValid) return;

        const formData = new FormData(form);
        const res = await fetch(form.action, {
            method: form.method,
            body: formData
        });

        if (res.status === 400) {
            const data = await res.json();

            if (data.errors) {
                Object.keys(data.errors).forEach(key => {
                    const input = form.querySelector(`[name="${key}"]`);
                    if (input) input.classList.add("input-validation-error");

                    const span = form.querySelector(`[data-valmsg-for="${key}"]`);
                    if (span) {
                        span.textContent = data.errors[key][0];
                        span.classList.add("field-validation-error");
                    }
                });
            }

            if (data.globalError) {
                const alertBox = document.querySelector('.alert-notification');
                if (alertBox) {
                    alertBox.innerText = data.globalError;
                    alertBox.classList.add('error');
                    alertBox.style.display = 'block';
                }
            }

            return;
        }

        if (res.redirected) {
            window.location.href = res.url;
        }
    });
}

// ========== INITIALIZE ON PAGE LOAD ==========
document.addEventListener("DOMContentLoaded", () => {
    document.querySelectorAll("form").forEach(attachFormValidation);

    //  MODAL OPEN 
    document.querySelectorAll('[data-modal="true"]').forEach(button => {
        button.addEventListener("click", () => {
            const modal = document.querySelector(button.getAttribute("data-target"));
            if (modal) modal.style.display = "flex";
        });
    });

    //  MODAL CLOSE 
    document.querySelectorAll('[data-close="true"]').forEach(button => {
        button.addEventListener("click", () => {
            const modal = button.closest(".modal");
            if (!modal) return;
            modal.style.display = "none";

            modal.querySelectorAll("form").forEach(form => {
                form.reset();
                const preview = form.querySelector(".image-preview");
                if (preview) preview.src = "";
                const wrapper = form.querySelector(".image-previewer");
                if (wrapper) wrapper.classList.remove("selected");
            });
        });
    });

    // IMAGE PREVIEWER
    document.querySelectorAll(".image-previewer").forEach(previewer => {
        const fileInput = previewer.querySelector("input[type='file']");
        const imgPreview = previewer.querySelector(".image-preview");

        previewer.addEventListener("click", () => fileInput.click());

        fileInput.addEventListener("change", ({ target: { files } }) => {
            const file = files[0];
            if (file) processImage(file, imgPreview, previewer);
        });
    });

    //  DYNAMIC UPDATE FORMS 
    document.querySelectorAll(".btn-edit-user").forEach(button => {
        button.addEventListener("click", () => {
            const userId = button.dataset.userId;
            const modal = document.querySelector("#editUserProfileModal");
            const content = modal.querySelector(".modal-content");

            fetch(`/users/edit/${userId}`)
                .then(res => res.text())
                .then(html => {
                    content.innerHTML = html;
                    modal.style.display = "flex";

                    const form = modal.querySelector("form");
                    attachFormValidation(form);

                    const previewer = modal.querySelector(".image-previewer");
                    if (previewer) attachImagePreviewer(previewer);


                    attachModalClose(modal);
                })
                .catch(err => {
                    content.innerHTML = "<p>Error loading form.</p>";
                    modal.style.display = 'flex';
                    console.error(err);
                });
        });
    });
});

document.querySelectorAll(".btn-edit-client").forEach(button => {
    button.addEventListener("click", () => {
        const clientId = button.dataset.userId;
        const modal = document.querySelector("#editClientModal");
        const content = modal.querySelector(".modal-content");

        fetch(`/clients/edit/${clientId}`)
            .then(res => res.text())
            .then(html => {
                content.innerHTML = html;
                modal.style.display = "flex";

                const form = modal.querySelector("form");
                attachFormValidation(form);
                attachModalClose(modal);

                setTimeout(() => {
                    const desc = modal.querySelector('#Description');
                    initWysiwyg(
                        '#project-wysiwyg-editor',
                        '#project-wysiwyg-toolbar',
                        '#Description',
                        desc?.value ?? ""
                    );
                }, 100);
            })
            .catch(err => {
                content.innerHTML = "<p>Error loading form.</p>";
                modal.style.display = "flex";
                console.error(err);
            });
    });
});

document.querySelectorAll(".btn-edit-project").forEach(button => {
    button.addEventListener("click", () => {
        const projectId = button.dataset.projectId;
        const modal = document.querySelector("#editProjectModal");
        const content = modal.querySelector(".modal-content");

        fetch(`/projects/edit/${projectId}`)
            .then(res => res.text())
            .then(html => {
                content.innerHTML = html;
                modal.style.display = "flex";

                const form = modal.querySelector("form");
                attachFormValidation(form);

                const previewer = modal.querySelector(".image-previewer");
                if (previewer) attachImagePreviewer(previewer);

                attachModalClose(modal);
                

                requestAnimationFrame(() => {
                    initWysiwyg(
                        '#update-project-wysiwyg-editor',
                        '#update-project-wysiwyg-toolbar',
                        '#update-description',
                        modal.querySelector('#update-description')?.value ?? ""
                    );
                

                const preSelectedUsers = JSON.parse(button.dataset.preSelectedUsers || "[]");

                    initTagSelector({
                        containerId: 'tagged-users-edit',
                        inputId: 'user-search-edit',
                        resultsId: 'user-search-results-edit',
                        searchUrl: (query) => 'users/search?searchTerm=' + encodeURIComponent(query),
                        displayProperty: 'fullName',
                        imageProperty: 'profilePicture',
                        tagClass: 'user-tag',
                        emptyMessage: 'No members found.',
                        preSelected: preSelectedUsers,
                        selectedInputIds: 'ProjectWithUsersRaw'
                    });
                });
            })
            .catch(err => {
                content.innerHTML = "<p>Error loading form.</p>";
                modal.style.display = "flex";
                console.error(err);
            });
    });
});

document.querySelectorAll(".btn-edit-status").forEach(button => {
    button.addEventListener("click", () => {
        const projectId = button.dataset.projectId;
        const modal = document.querySelector("#editProjectStatusModal");
        const content = modal.querySelector(".modal-content");

        fetch(`/projects/status/${projectId}`)
            .then(res => res.text())
            .then(html => {
                content.innerHTML = html;
                modal.style.display = "flex";

                const form = modal.querySelector("form");
                attachFormValidation(form);
                attachModalClose(modal);
            })
            .catch(err => {
                content.innerHTML = "<p>Error loading form.</p>";
                modal.style.display = "flex";
                console.error(err);
            });
    });
});

// IMAGE PREVIEW FUNCTIONS 
async function loadImage(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.onerror = () => reject("Failed to load file");
        reader.onload = e => {
            const img = new Image();
            img.onerror = () => reject("Invalid image");
            img.onload = () => resolve(img);
            img.src = e.target.result;
        };
        reader.readAsDataURL(file);
    });
}

async function processImage(file, imagePreview, previewer, previewerSize = previewSize) {
    try {
        const img = await loadImage(file);
        const canvas = document.createElement("canvas");
        canvas.width = previewerSize;
        canvas.height = previewerSize;
        const ctx = canvas.getContext("2d");
        ctx.drawImage(img, 0, 0, previewerSize, previewerSize);
        imagePreview.src = canvas.toDataURL("image/jpeg");
        previewer.classList.add("selected");
    } catch (err) {
        console.error("Image preview error:", err);
    }
}


// Close Modal func
function attachModalClose(modal) {
    const closeButton = modal.querySelector('[data-close="true"]');
    if (closeButton) {
        closeButton.addEventListener('click', () => {
            modal.style.display = 'none';
        });
    }
}

// Add image for another form
function attachImagePreviewer(previewer) {
    const fileInput = previewer.querySelector("input[type='file']");
    const imgPreview = previewer.querySelector(".image-preview");

    if (!fileInput || !imgPreview) return;

    previewer.addEventListener("click", () => fileInput.click());

    fileInput.addEventListener("change", ({ target: { files } }) => {
        const file = files[0];
        if (file) processImage(file, imgPreview, previewer);
    });
}

// Dropdown

document.addEventListener('DOMContentLoaded', () => {
    const dropdowns = document.querySelectorAll('[data-type="dropdown"]');

    document.addEventListener('click', (e) => {
        let clickedDropdown = null;

        dropdowns.forEach(dropdown => {
            const targetId = dropdown.getAttribute('data-target');
            const targetElement = document.querySelector(targetId);

            if (dropdown.contains(e.target)) {
                clickedDropdown = targetElement;

                document.querySelectorAll('.dropdown.dropdown-show').forEach(openDropdown => {
                    if (openDropdown !== targetElement) {
                        openDropdown.classList.remove('dropdown-show');
                    }
                });

                targetElement.classList.toggle('dropdown-show');
            }
        });
        if (!clickedDropdown) {
            document.querySelectorAll('.dropdown.dropdown-show').forEach(openDropdown => {
                openDropdown.classList.remove('dropdown-show');
            });
        }
    });
});


document.querySelectorAll(".dropdown-action.trash.project-delete").forEach(btn => {
    btn.addEventListener("click", (e) => {
        e.preventDefault();
        const id = btn.dataset.projectId;

        if (confirm("Are you sure you want to delete this project?")) {
            fetch(`/projects/delete/${id}`, {
                method: 'POST'
            }).then(res => {
                if (res.ok) {
                    location.reload();
                } else {
                    alert("Failed to delete project.");
                }
            });
        }
    });
});

document.querySelectorAll(".dropdown-action.trash.user-delete").forEach(btn => {
    btn.addEventListener("click", (e) => {
        e.preventDefault();
        const id = btn.dataset.userId;

        if (confirm("Are you sure you want to delete this user?")) {
            fetch(`/users/delete/${id}`, {
                method: 'POST'
            }).then(res => {
                if (res.ok) {
                    location.reload();
                } else {
                    alert("Failed to delete user.");
                }
            });
        }
    });
});

function updateRelativeTimes() {
    const elements = document.querySelectorAll('.notification .time');
    const now = new Date();

    elements.forEach(el => {
        const created = new Date(el.getAttribute('data-created'));
        const diff = now - created;
        const diffSeconds = Math.floor(diff / 1000);
        const diffMinutes = Math.floor(diffSeconds / 60);
        const diffHours = Math.floor(diffMinutes / 60);
        const diffDays = Math.floor(diffHours / 24);
        const diffWeeks = Math.floor(diffDays / 7);

        let relativeTime = '';

        if (diffMinutes < 1) {
            relativeTime = '0 min ago';
        } else if (diffMinutes < 60) {
            relativeTime = diffMinutes + ' min ago';
        } else if (diffHours < 2) {
            relativeTime = diffHours + ' hour ago';
        } else if (diffHours < 24) {
            relativeTime = diffHours + ' hours ago';
        } else if (diffDays < 2) {
            relativeTime = diffDays + ' day ago';
        } else if (diffDays < 7) {
            relativeTime = diffDays + ' days ago';
        } else {
            relativeTime = diffWeeks + ' weeks ago';
        }
        el.textContent = relativeTime;
    });
}