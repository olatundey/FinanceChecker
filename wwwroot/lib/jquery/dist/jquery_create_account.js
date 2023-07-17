$(document).ready(function () {
    // Function to show or hide fields based on the selected sync type
    function toggleFields() {
        var syncType = $('input[name="syncType"]:checked').val();

        if (syncType === "manual") {
            $('#institutionNameField').show();   
            $('#balanceField').show();
            $('#submitField').show();
            $('#validateField').hide();
        } else if (syncType === "automatic") {
            $('#institutionNameField').show();
            $('#balanceField').hide();
            $('#submitField').hide();
            $('#validateField').show();
            updateInstitutionOptions(); // Update the institution options immediately when the sync type is set to automatic
        }
    }

    // Initial toggle when the page loads
    toggleFields();

    // Toggle fields when sync type is changed
    $('input[name="syncType"]').change(function () {
        toggleFields();
    });

    // Toggle institution name field when account type is changed
    $('#AccountType').change(function () {
        var syncType = $('input[name="syncType"]:checked').val();

        if (syncType === "automatic") {
            updateInstitutionOptions(); // Update the institution options when the account type is changed
        }
    });

    // Function to update the institution options based on the selected account type
    function updateInstitutionOptions() {
        var accountType = $('#AccountType').val();

        if (accountType === "BankAccount") {
            $('#institutionNameField').html(`
                <label for="institutionName">Institution Name:</label>
                <select class="form-control" name="InstitutionName">
                    <option value="BankA">Bank A</option>
                    <option value="BankB">Bank B</option>
                </select>
            `);
            $('#institutionNameField').show();
        } else if (accountType === "Investment") {
            $('#institutionNameField').html(`
                <label for="institutionName">Institution Name:</label>
                <select class="form-control" name="InstitutionName">
                    <option value="InvestmentA">Investment A</option>
                </select>
            `);
            $('#institutionNameField').show();
        } else if (accountType === "CreditCard") {
            $('#institutionNameField').html(`
                <label for="institutionName">Institution Name:</label>
                <select class="form-control" name="InstitutionName">
                    <option value="CreditA">Credit A</option>
                    <option value="CreditB">Credit B</option>
                </select>
            `);
            $('#institutionNameField').show();
        } else {
            $('#institutionNameField').hide();
        }
    }

    // Get the selected account type from the Model
    var selectedAccountType = "@Model.AccountType";

    // Set the selected account type in the AccountType select element
    $('#AccountType').val(selectedAccountType);

    // Trigger the change event to update the institution options
    $('#AccountType').change();
});

