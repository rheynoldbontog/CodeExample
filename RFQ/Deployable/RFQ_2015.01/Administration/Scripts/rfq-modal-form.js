// modalform.js

$(function () {
    $.ajaxSetup({ cache: false });
    //ModalClickBind();
    rfqModal.modalClickBind();
});

var rfqModal = (function () {
    "use strict"

    var myModalLink = "a[rfq-modal]";
    var myModal = '#myModal';
    var myModalContent = '#myModalContent';
    var myModalFormId = '#modal_form';
    var formTypeName = "[name='initial_form_type']";
    var generalFormFieldsName = '.general_form_optional';
    var techFormOptional = '.technical_form_optional';
    var purchaseTypeId = '#TestEquipmentPurchaseTypeId';
    var calibrationTypeId = '#TestEquipmentCalibrationTypeId';
    var testEquipmentApplicationId = '#TestEquipmentApplication';
    var dataValRequiredAttribute = 'data-val-required';
    var dataValLengthAttribute = 'data-val-length';
    var dataValLengthMax = 'data-val-length-max';
    var purchaseTypeRequired = "Test Equipment Purchase Type is required";
    var calibrationTypeRequired = "Test Equipment Purchase Type is required";
    var testEquipmentApplicationLength = "'Test Equipment Application' must be between 0 and 50 characters";
    var testEquipmentApplicationMax = "100";

    var lastQuotationId = '#last_quotation_index';
    var quotationTabHeaderContainer = "#quotation_tabs";
    var quotationTabItemContainer = "#quotation_tabs_content";

    var showGeneralFormFields = function () {
        $(generalFormFieldsName).show();
        $(techFormOptional).hide();
        $(purchaseTypeId).removeAttr(dataValRequiredAttribute);
        $(calibrationTypeId).removeAttr(dataValRequiredAttribute);
        $(testEquipmentApplicationId).removeAttr(dataValLengthAttribute);
        $(testEquipmentApplicationId).removeAttr(dataValLengthMax);
    };

    var showTechnicalFormFields = function () {
        $(generalFormFieldsName).hide();
        $(techFormOptional).show();

        $(purchaseTypeId).attr(dataValRequiredAttribute, purchaseTypeRequired);
        $(calibrationTypeId).attr(dataValRequiredAttribute, calibrationTypeRequired);
        $(testEquipmentApplicationId).attr(dataValLengthAttribute, testEquipmentApplicationLength);
        $(testEquipmentApplicationId).attr(dataValLengthMax, testEquipmentApplicationMax);
    };

    var initializeForm = function () {
        var initialFormType = $(formTypeName).val();

        // General Form Type
        if (initialFormType == 1) {
            showGeneralFormFields();
        }
        else {
            showTechnicalFormFields();
        }

        // Initialize tabs...
        $('#tabs').tab();

        // Initialize file uploader items...
        $('.fu_container').fu();

        //Parse form...
        parseFormForValidation(myModalFormId);

        //Adjust modal height and width
        resizeModal();

        //Tooltips...
        $('[data-toggle="tooltip"]').tooltip();

        //Initialize typeaheads...
        initializeTypeAheadData();
    };

    var bindForm = function (dialog) {
        $('form', dialog).submit(function () {

            if ($(this).valid()) {

                var actionUrl = this.action;
                var methodType = this.method;
                var formData = $(this).serialize();

                swal({
                    title: 'Are you sure?',
                    text: "Line will be saved to the database!",
                    type: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#303F9F',
                    cancelButtonColor: '#f39c12',
                    confirmButtonText: 'Yes, save line!'
                }).then(function () {
                    //Removes validation summary
                    removeValidationSummary();

                    //AJAX CALL HERE...
                    $.ajax({
                        url: actionUrl,
                        type: methodType,
                        data: formData,
                        success: function (result) {
                            if (result.success) {
                                swal({
                                    title: 'Good job!',
                                    text: 'RFQ Line has been saved!',
                                    timer: 3000,
                                    type: 'success'
                                }).then(
                                function () {
                                    $(myModal).modal('hide');
                                    window.location.href = window.location.href;
                                },
                                // handling the promise rejection
                                function (dismiss) {
                                    $(myModal).modal('hide');
                                    window.location.href = window.location.href;
                                });

                            } else {
                                $(myModalContent).html(result);

                                initializeForm();
                                bindForm(dialog);
                            }
                        }
                    });
                }, function (dismiss) {

                });
            }
            return false;
        });
    };

    var modalClickBind = function () {

        $(myModalLink).on("click", function (e) {
            // hide dropdown if any (this is used wehen invoking modal from link in bootstrap dropdown )
            //$(e.target).closest('.btn-group').children('.dropdown-toggle').dropdown('toggle');

            $(myModalContent).load(this.href, function () {
                $(myModal).modal({
                    /*backdrop: 'static',*/
                    keyboard: true
                }, 'show');

                initializeForm();
                bindForm(this);
            });
            return false;
        });
    };

    var changeFormType = function (formType) {

        var value = parseInt(formType.options[formType.selectedIndex].value);

        if (value === 1) {
            showGeneralFormFields();
        }
        else if (value === 2) {
            showTechnicalFormFields();
        }

        parseFormForValidation(myModalFormId);
    };

    var submitForm = function (submitUrl) {

        if ($('form').valid()) {
            swal({
                title: 'Are you sure?',
                text: "RFQ status will be changed to submitted!",
                type: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#303F9F',
                cancelButtonColor: '#f39c12',
                confirmButtonText: 'Yes, submit RFQ!'
            }).then(function () {
                //AJAX CALL HERE...
                $.ajax({
                    url: submitUrl,
                    type: 'GET',
                    data: null,
                    success: function (result) {
                        if (result.success) {
                            swal({
                                title: 'Good job!',
                                text: 'RFQ has been submitted! The requester will recieve an email notification!!',
                                timer: 3000,
                                type: 'success'
                            }).then(
                            function () {
                                window.location.href = window.location.href;
                            },
                            // handling the promise rejection
                            function (dismiss) {
                                window.location.href = window.location.href;
                            });

                        } else {
                            swal('Ooops...', result.errorMessage, 'error');
                        }
                    }
                });
            }, function (dismiss) {

            });
        }
    };

    var deleteLine = function (deleteUrl) {
        swal({
            title: 'Are you sure?',
            text: "Line will be marked as deleted in the database and will no longer be visible!",
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#303F9F',
            cancelButtonColor: '#f39c12',
            confirmButtonText: 'Yes, delete line!'
        }).then(function () {
            //AJAX CALL HERE...
            $.ajax({
                url: deleteUrl,
                type: 'GET',
                data: null,
                success: function (result) {
                    if (result.success) {
                        swal({
                            title: 'Good job!',
                            text: 'Line has been deleted! The requester and buyer will recieve an email notification!',
                            timer: 3000,
                            type: 'success'
                        }).then(
                        function () {
                            window.location.href = window.location.href;
                        },
                        // handling the promise rejection
                        function (dismiss) {
                            window.location.href = window.location.href;
                        });

                    } else {
                        swal('Ooops...', result.errorMessage, 'error');
                    }
                }
            });
        }, function (dismiss) {

        });
    };

    var createQuotation = function (createUrl) {

        // Get last index...
        var last_quotation_index = $(lastQuotationId).val();

        // Add index to url...
        createUrl += '&lastIndex=' + last_quotation_index;

        $.ajax({
            url: createUrl,
            type: 'GET',
            data: null,
            cache: false,
            success: function (result) {
                if (result.success) {
                    // Increment last index...
                    var currentIndex = parseInt(last_quotation_index) + 1;
                    $(lastQuotationId).val(currentIndex);

                    // Append Tab Header partial...
                    $(quotationTabHeaderContainer).append(result.tabHeaderItem);

                    // Append Tab Content partial...
                    $(quotationTabItemContainer).append(result.tabContentItem);

                    // Initialize tabs...
                    $('#tabs').tab();

                    // Initialize file uploader items...
                    $('.fu_container').fu();

                    // Parse form for validation...
                    removeValidationSummary();
                    parseFormForValidation(myModalFormId);

                    // Show delete button...
                    $('#delete-quotation').show();

                    activateQuotationTab(last_quotation_index);

                    // Initialize TypeAhead...
                    initializeTypeAheadData();
                }
                else
                    swal('Ooops...', result.errorMessage, 'error');
            }
        });
    };

    var initializeTypeAheadData = function () {

        $('.ajax-typeahead').typeahead({
            source: function (query, process) {
                var options = [];

                return $.ajax({
                    url: this.$element.data('link'),
                    type: 'get',
                    data: { query: query },
                    dataType: 'json',
                    success: function (data) {

                        if (data.options)
                            process(data.options);
                    }
                });
            }
        });
    }

    var getActiveQuotationTab = function () {
        var tabIndex = null;

        $('#quotation_tabs').find("li.active").each(function (index, obj) {
            tabIndex = parseInt($(this).attr('tabindex'));

            //Check if deleted...
            var isDeleted = $('#Quotations_' + tabIndex + '__IsDeleted').val();

            if (isDeleted === 'True' || isDeleted === 'true') {
                tabIndex = null;
            };
        });

        return tabIndex;
    };

    var activateQuotationTab = function (index) {
        $('.nav.nav-pills a[href="#quotation' + index + '"]').tab('show')
    };

    var activateFirstNonDeletedTab = function () {

        $('#quotation_tabs').find("li").each(function (index, obj) {
            var tabIndex = parseInt($(this).attr('tabindex'));

            //Check if deleted...
            var isDeleted = $('#Quotations_' + tabIndex + '__IsDeleted').val();

            if (isDeleted === 'False' || isDeleted === 'false') {
                activateQuotationTab(tabIndex);
            };
        });
    };

    var markQuotationAsDeleted = function (tabIndex) {

        $('#Quotations_' + tabIndex + '__IsDeleted').val(true);

        //Make sure required fields have value so that they will not be flagged as error
        //by Jquery validation...
        $("#Quotations_" + tabIndex + "__Supplier").val("**MARKED AS DELETED**");
        $("#Quotations_" + tabIndex + "__Commodity").val("**MARKED AS DELETED**");
        $("#Quotations_" + tabIndex + "__QuoteItemDescription").val("**MARKED AS DELETED**");
        $("#Quotations_" + tabIndex + "__CurrencyId").val(1);
        $("#Quotations_" + tabIndex + "__UnitPrice").val(1);

        $('#tabHeader_' + tabIndex).fadeOut();
        $('#quotation' + tabIndex).fadeOut();

        activateFirstNonDeletedTab();

    };

    var deleteQuotation = function () {
        var activeTab = getActiveQuotationTab();

        //Check if quotation is already saved or has id > zero
        var quoteId = $('#Quotations_' + activeTab + '__Id').val();
        
        if (quoteId && quoteId == 0) {
            if ($.isNumeric(activeTab)) {
                swal({
                    title: 'Are you sure?',
                    text: "Quotation will be marked as deleted and will no longer be visible!",
                    type: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#303F9F',
                    cancelButtonColor: '#f39c12',
                    confirmButtonText: 'Yes, delete quotation!'
                }).then(function () {

                    markQuotationAsDeleted(activeTab);
                    swal('Good job!', 'Quotation has been marked as deleted!', 'success');

                }, function (dismiss) {

                });
            }
            else {
                swal('Ooops...', 'Sorry, Cannot mark this tab as deleted! It may have been marked as deleted already!', 'error');
            };
        }
        else {
            swal('Ooops...', 'Sorry, Cannot mark saved quotation as deleted!', 'error');
        }
    };

    return {
        modalClickBind: modalClickBind,
        changeFormType: changeFormType,
        submitForm: submitForm,
        deleteLine: deleteLine,
        createQuotation: createQuotation,
        deleteQuotation: deleteQuotation,
        getActiveQuotationTab: getActiveQuotationTab,
        initializeTypeAheadData: initializeTypeAheadData
    };

})();


function removeValidationSummary() {
    $('.validation-summary-errors').addClass('validation-summary-valid');
    $('.validation-summary-errors').removeClass('validation-summary-errors');
}

function resizeModal() {

    var userRole = $('a[rfq-modal]').attr('data-user-role');
    $('body .modal').css('width', '97%');
    $('body .modal').css('margin-left', '-48.5%');
    $('body .modal').css('top', '2%');
    $('body .modal .modal-content').css('height', '80%');
    $('body .modal .modal-body').css('overflow-y', 'auto');
    $('body .modal .modal-body').css('max-height', 'calc(100vh - 210px)');
    $('form#modal_form.form-horizontal').css('margin-bottom', '0px');
}

function parseFormForValidation(formId) {
    var form = $(formId)
               .removeData("validator")
               .removeData("unobtrusiveValidation");

    $.validator.unobtrusive.parse(form);
    form.data("validator").settings.ignore = "";
}
