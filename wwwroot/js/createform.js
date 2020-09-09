var options = {
    disabledActionButtons: ['data'],
    disableFields: ['autocomplete', 'button', 'date', 'file', 'header', 'hidden', 'number', 'paragraph', 'starRating', 'text', 'textarea'],
    onSave: function (evt, formData) {
        console.log("formbuilder saved");
        window.sessionStorage.setItem('formData', formData);
        document.getElementById('optionFormData').value = sessionStorage.getItem("formData");
        toggleEdit(false);
        $(".render-wrap").formRender({ formData });
    },
};

function toggleEdit(editing) {
    document.body.classList.toggle("form-rendered", !editing);
};

document.getElementById("edit-form").onclick = function () {
    toggleEdit(true);
};

jQuery(function ($) {
    $(document.getElementById('fb-editor')).formBuilder(options);
});