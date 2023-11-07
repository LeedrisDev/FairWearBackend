// JavaScript to handle category and range creation, deletion, and form submission
document.addEventListener("DOMContentLoaded", function () {
    const categoriesInput = document.getElementById("categories-input");
    const categoriesList = document.getElementById("categories-list");
    const addCategoryButton = document.getElementById("add-category");
    const hiddenCategoriesInputsParent = document.getElementById("categories-inputs");
    const categoryInputs = Array.from(document.querySelectorAll('input[name*="Brand.Categories"]'));
    let categories = []; // Array to store created categories

    const rangesInput = document.getElementById("ranges-input");
    const rangesList = document.getElementById("ranges-list");
    const addRangeButton = document.getElementById("add-range");
    const hiddenRangesInputsParent = document.getElementById("ranges-inputs");
    const rangeInputs = Array.from(document.querySelectorAll('input[name*="Brand.Ranges"]'));
    let ranges = []; // Array to store created ranges

    const saveButton = document.getElementById("save-button");

    /**
     * Adds a category to a list of categories and displays it in the DOM.
     *
     * @param {string} text - The text of the category to be added.
     */
    function addCategory(text) {
        if (text === "")
            return

        const categoryElement = document.createElement("div");
        categoryElement.className = "category d-inline-flex mb-2";

        // Create the colored rectangle element
        const categoryRectangle = document.createElement("div");
        categoryRectangle.className = "category-rectangle";
        categoryElement.appendChild(categoryRectangle);

        // Create the text element
        const categoryTextElement = document.createElement("span");
        categoryTextElement.className = "category-text margin-y-0";
        categoryTextElement.textContent = text;
        categoryRectangle.appendChild(categoryTextElement);

        // Create the delete button
        const deleteButton = document.createElement("button");
        deleteButton.className = "delete-category";
        categoryRectangle.appendChild(deleteButton);
        deleteButton.addEventListener("click", function () {
            // Remove the category text from the categories array
            const categoryIndex = categories.indexOf(text);

            // Remove the category element from the DOM
            categoryElement.remove();

            if (categoryIndex !== -1)
                categories.splice(categoryIndex, 1);
        });

        // Add the ticket to the list
        categoriesList.appendChild(categoryElement);
        categoriesInput.value = "";
        categories.push(text);
        
    }

    /**
     * Adds a range to a list of ranges and displays it in the DOM.
     *
     * @param {string} text - The text of the range to be added.
     */
    function addRange(text) {
        if (text === "")
            return

        const rangeElement = document.createElement("div");
        rangeElement.className = "range d-inline-flex mb-2";

        // Create the colored rectangle element
        const rangeRectangle = document.createElement("div");
        rangeRectangle.className = "range-rectangle";
        rangeElement.appendChild(rangeRectangle);

        // Create the text element
        const rangeTextElement = document.createElement("span");
        rangeTextElement.className = "range-text margin-y-0";
        rangeTextElement.textContent = text;
        rangeRectangle.appendChild(rangeTextElement);

        // Create the delete button
        const deleteButton = document.createElement("button");
        deleteButton.className = "delete-range";
        rangeRectangle.appendChild(deleteButton);
        deleteButton.addEventListener("click", function () {
            // Remove the range element from the DOM
            rangeElement.remove();

            // Remove the range text from the ranges array
            const rangeIndex = ranges.indexOf(text);
            if (rangeIndex !== -1) {
                ranges.splice(rangeIndex, 1);
            }
        });

        // Add the range to the list
        rangesList.appendChild(rangeElement);
        rangesInput.value = "";
        ranges.push(text);
    }

    /**
     * Adds an input value as a category or range if it is not empty.
     *
     * @param {string} inputValue - The value of the input to be added.
     * @param {function} addFunction - The function to add the input value (e.g., addCategory or addRange).
     */
    function addInput(inputValue, addFunction) {
        if (inputValue.trim() !== "") {
            addFunction(inputValue);
        }
    }


    // Handle "Add" button click for categories
    addCategoryButton.addEventListener("click", function () {
        const categoryText = categoriesInput.value.trim();
        addInput(categoryText, addCategory);
    });

    // Handle "Add" button click for ranges
    addRangeButton.addEventListener("click", function () {
        const rangeText = rangesInput.value.trim();
        addInput(rangeText, addRange);
    });

    // Add existing category inputs
    for (let i = 0; i < categoryInputs.length; i++) {
        const input = categoryInputs[i];
        addInput(input.value, addCategory);
        input.remove();
    }

    // Add existing range inputs
    for (let i = 0; i < rangeInputs.length; i++) {
        const input = rangeInputs[i];
        addInput(input.value, addRange);
        input.remove();
    }


    /**
     * Creates a hidden input element and appends it to a parent HTML element.
     *
     * @param {HTMLElement} parentNode - The HTML element to which the hidden input will be appended.
     * @param {string} name - The name attribute for the hidden input.
     * @param {string} value - The value attribute for the hidden input.
     */
    function createHiddenInput(parentNode, name, value) {
        let inputElement = document.createElement("input");
        inputElement.type = "hidden";
        inputElement.className = "form-control";
        inputElement.name = name;
        inputElement.value = value;
        parentNode.appendChild(inputElement);
    }
    
    saveButton.addEventListener("click", function () {
        // Add hidden inputs for categories
        if (categories.length > 0) {
            categories.forEach((value, i) => {
                const name = `Brand.Categories[${i}]`;
                createHiddenInput(hiddenCategoriesInputsParent, name, value);
            });
        }

        // Add hidden inputs for ranges
        if (ranges.length > 0) {
            ranges.forEach((value, i) => {
                const name = `Brand.Ranges[${i}]`;
                createHiddenInput(hiddenRangesInputsParent, name, value);
            });
        }

    });
});