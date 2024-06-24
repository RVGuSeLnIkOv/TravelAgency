<script>
    import html2canvas from 'html2canvas';
    import { onMount } from 'svelte';

    import 'tailwindcss/base';
    import 'tailwindcss/components';
    import 'tailwindcss/utilities';
    import 'daisyui/dist/full.css';

    import { navigate } from 'svelte-routing';

    // Метод проверки целого числа
  function checkingInt(input, minValue, maxValue) {
    try {
      input = input.trim();
      let output = parseInt(input, 10);
      if (output < minValue || output > maxValue)
        return -1;
      return output;
    } catch {
      return -1;
    }
  }

    let employee = null;

    let selectedAdults = [];
    let selectedChildren = [];

    let countries = [];
    let cities = [];
    let residences = [];
    let tourOperators = [];
    let typesMeal = [];

    let customerSelect = '';
    let inputCost = 0;
    let countrySelect = '';
    let citySelect = '';
    let residenceSelect = '';
    let chekinDate = '';
    let chekoutDate = '';
    let operatorSelect = '';
    let typeMealSelect = '';

    let successMessage = '';
    let errorMessage = '';

    const { state } = history;
    if (state && state.selectedAdults) {
        selectedAdults = state.selectedAdults;
        selectedChildren = state.selectedChildren;
    }

    if (state && state.employee) {
        employee = state.employee;
    }
    
    onMount(() => {
        fetchCountries();
        fetchTourOperators();
        fetchTypesMeal();
    });

    function toAuthClick() {
        navigate(`/`);
    }

    function toMainClick() {
        navigate(`/administratorAccount`, { state: { employee } });
    }

    function toTouristsClick() {
        navigate(`/tourists`, { state: { employee } });
    }

    function toToursBookingsClick() {
        navigate(`/toursBookings`, { state: { employee } });
    }

    function closeEForm() {
        errorMessage = '';
    }

    function closeSForm() {
        successMessage = '';
    }

    async function fetchCountries() {
        fetch(`http://localhost:5000/api/Country/Countries`)
            .then(response => {
                if (response.ok) {
                return response.json();
                } else {
                    errorMessage = 'Ошибка при выполнении запроса';
                }
            })
            .then(data => {
                countries = data;
            })
            .catch(error => {
                errorMessage = 'Ошибка при выполнении запроса';
            });
    }

    async function fetchCities(selectedCountryId) {
        document.getElementById('citySelect').selectedIndex = 0;
        document.getElementById('residenceSelect').selectedIndex = 0;
        residences = [];
        fetch(`http://localhost:5000/api/Country/${selectedCountryId}/Cities`)
            .then(response => {
                if (response.ok) {
                return response.json();
                } else {
                    errorMessage = 'Ошибка при выполнении запроса';
                }
            })
            .then(data => {
                cities = data;
            })
            .catch(error => {
                errorMessage = 'Ошибка при выполнении запроса';
            });
    }

    async function fetchResidences(selectedCityId) {
        document.getElementById('residenceSelect').selectedIndex = 0;
        fetch(`http://localhost:5000/api/City/${selectedCityId}/Residences`)
            .then(response => {
                if (response.ok) {
                return response.json();
                } else {
                    errorMessage = 'Ошибка при выполнении запроса';
                }
            })
            .then(data => {
                residences = data;
            })
            .catch(error => {
                errorMessage = 'Ошибка при выполнении запроса';
            });
    }

    async function fetchTourOperators() {
        fetch(`http://localhost:5000/api/TourOperator/TourOperators`)
            .then(response => {
                if (response.ok) {
                return response.json();
                } else {
                    errorMessage = 'Ошибка при выполнении запроса';
                }
            })
            .then(data => {
                tourOperators = data;
            })
            .catch(error => {
                errorMessage = 'Ошибка при выполнении запроса';
            });
    }

    async function fetchTypesMeal() {
        fetch(`http://localhost:5000/api/TypeMeal/TypesMeal`)
            .then(response => {
                if (response.ok) {
                return response.json();
                } else {
                    errorMessage = 'Ошибка при выполнении запроса';
                }
            })
            .then(data => {
                typesMeal = data;
            })
            .catch(error => {
                errorMessage = 'Ошибка при выполнении запроса';
            });
    }

    async function createTour() {
        const tourists = [...selectedAdults, ...selectedChildren];
        const tourCreate = {
            idTour: 0,
            idTourOperator: parseInt(operatorSelect, 10),
            idResidence: parseInt(residenceSelect, 10),
            idTypeMeal: parseInt(typeMealSelect, 10),
            checkinDate: chekinDate,
            checkoutDate: chekoutDate,
            duration: 0,
            notes: "",
            isArchive: false
        }

        const touristParams = tourists.map(tourist => `tourists=${encodeURIComponent(tourist.idTourist)}`).join('&');
        const url = `http://localhost:5000/api/Tour?${touristParams}`;

        try {
        const response = await fetch(url, {
            method: "POST",
            headers: {
            "Content-Type": "application/json; charset=utf-8"
            },
            body: JSON.stringify(tourCreate)
        });

        if (response.ok) {
            const result = await response.json();
            if (result > 0) {
                const parts = new Date().toLocaleDateString().split(".");
                const formattedDate = `${parts[2]}-${parts[1]}-${parts[0]}`;
                const bookingCreate = {
                    idBooking: 0,
                    idTour: result,
                    idTourist: parseInt(customerSelect, 10),
                    cost: parseInt(inputCost, 10),
                    idEmployee: employee.idEmployee,
                    date: formattedDate
                };
                try {
                    const response = await fetch('http://localhost:5000/api/Booking', {
                    method: 'POST',
                    headers: {
                    'Content-Type': 'application/json; charset=utf-8'
                    },
                    body: JSON.stringify(bookingCreate)
                });
                    if (response.ok) {
                        takeScreenshot();
                        successMessage = 'Тур и заказ на тур успешно добавлены!';
                        setTimeout(() => {
                        navigate(`/administratorAccount`, { state: { employee } });
                    }   , 2000);
                    } else {
                        errorMessage = 'Ошибка при выполнении запроса';
                    } 
                } catch (error) {
                    errorMessage = 'Ошибка при выполнении запроса';
                }
            } else {
                errorMessage = 'Ошибка при выполнении запроса';
            }
        } else {
            errorMessage = 'Ошибка при выполнении запроса';
        }
        } catch (error) {
            errorMessage = 'Ошибка при выполнении запроса';
        }
    }

    function addTourClick(event) {
        event.preventDefault();
        clearStyle('customerSelect');
        clearStyle('inputCost')
        clearStyle('countrySelect')
        clearStyle('citySelect')
        clearStyle('residenceSelect')
        clearStyle('checkinDate');
        clearStyle('checkoutDate')
        clearStyle('operatorSelect')
        clearStyle('typeMealSelect')

        let isCorrect = true;

        inputCost = document.getElementById("inputCost").value;
        chekinDate = document.getElementById("checkinDate").value;
        chekoutDate = document.getElementById("checkoutDate").value;

        if (customerSelect === '') {
            isCorrect = false;
            errorStyle('customerSelect', 'Поле заполняется обязательно!');
        }
        if (countrySelect === '') {
            isCorrect = false;
            errorStyle('countrySelect', 'Поле заполняется обязательно!');
        }
        if (citySelect === '') {
            isCorrect = false;
            errorStyle('citySelect', 'Поле заполняется обязательно!');
        }
        if (residenceSelect === '') {
            isCorrect = false;
            errorStyle('residenceSelect', 'Поле заполняется обязательно!');
        }
        if (chekinDate === '') {
            isCorrect = false;
            errorStyle('checkinDate', 'Поле заполняется обязательно!');
        } else if (chekoutDate === '') {
            isCorrect = false;
            errorStyle('checkoutDate', 'Поле заполняется обязательно!');
        } else {
            var chekinDateObj = new Date(chekinDate);
            var chekoutDateObj = new Date(chekoutDate);

            if (chekinDateObj >= chekoutDateObj) {
                isCorrect = false;
                errorStyle('checkinDate', 'Дата заезда не может быть позже, чем дата выезда!');
                errorStyle('checkoutDate', 'Дата заезда не может быть позже, чем дата выезда!');
            }
        }
        if (operatorSelect === '') {
            isCorrect = false;
            errorStyle('operatorSelect', 'Поле заполняется обязательно!');
        }
        if (typeMealSelect === '') {
            isCorrect = false;
            errorStyle('typeMealSelect', 'Поле заполняется обязательно!');
        }
        if (isNaN(checkingInt(inputCost, 0, 10000000))) {
            isCorrect = false;
            errorStyle('inputCost', 'Поле заполнено неверно!');
        }
        

        if (isCorrect) {
            createTour();
        }
    }

    function handleCustomerChange(event) {
        customerSelect = event.target.value;
    }

    function handleCountryChange(event) {
        countrySelect = event.target.value;
    }

    function handleCityChange(event) {
        citySelect = event.target.value;
    }

    function handleResidenceChange(event) {
        residenceSelect = event.target.value;
    }

    function handleTourOperatorChange(event) {
        operatorSelect = event.target.value;
    }

    function handleTypeMealChange(event) {
        typeMealSelect = event.target.value;
    }

    async function takeScreenshot() {
        const element = document.getElementById('myId');
        const canvas = await html2canvas(element);
        const dataURL = canvas.toDataURL();
        const link = document.createElement('a');
        link.href = dataURL;
        link.download = 'screenshot.png';
        link.click();
    }

</script>

{#if successMessage}
    <div class="alert alert-success shadow-lg">
        <span>{successMessage}</span>
        <button class="btn btn-sm btn-circle absolute right-2 top-2" on:click={closeSForm}>✕</button>
    </div>
{/if}

{#if errorMessage}
    <div class="alert alert-error shadow-lg">
        <span>{errorMessage}</span>
        <button class="btn btn-sm btn-circle absolute right-2 top-2" on:click={closeEForm}>✕</button>
    </div>
{/if}

<body>
    <main>
        <div class="navbar bg-base-100">
            <div>
                <span>
                    <h3>АДМИНИСТРАТОР</h3>
                    <h3>{employee.surname} {employee.name[0]}.</h3>
                </span>
            </div>
            <div class='icons_buttons'>
                <button class="account btn btn-ghost" id="currentLink" on:click={toMainClick}>
                    <img src="/home.png" alt="Главная" class="icon" />
                    Главная
                </button>
                <button class="account btn btn-ghost" on:click={toTouristsClick}>
                    <img src="/tourist.png" alt="Туристы" class="icon" />
                    Туристы
                </button>
                <button class="account btn btn-ghost" on:click={toToursBookingsClick}>
                    <img src="/word.png" alt="Туры и заказы" class="icon" />
                    Туры и заказы
                </button>
            </div>
            <button class="exit btn btn-ghost" id="toAuth" on:click={toAuthClick}>Выход</button>
        </div>

        <div id='myId' class="border border-parameters">
            <form class="flex flex-col">
                <h4 class="no-margin">УЧАСТНИКИ ТУРА И СТОИМОСТЬ</h4>
                {#if selectedChildren.length !== 0}
                    <label for="adults">Туристы (взрослые)</label>
                {:else}
                    <label for="adults">Туристы</label>
                {/if}
                <div id="adults" class="overflow-x-auto">
                    <table class="table table-zebra w-full">
                        <thead>
                        <tr>
                            <th class="text-center">Фамилия</th>
                            <th class="text-center">Имя</th>
                            <th class="text-center">Отчество</th>
                            <th class="text-center">Дата рождения</th>
                            <th class="text-center">Пол</th>
                        </tr>
                        </thead>
                        <tbody>
                        {#each selectedAdults as tourist}
                            <tr>
                            <td class="text-center">{tourist.surname}</td>
                            <td class="text-center">{tourist.name}</td>
                            <td class="text-center">{tourist.patronymic}</td>
                            <td class="text-center">{tourist.birthDate.slice(0,10)}</td>
                            <td class="text-center">{tourist.gender}</td>
                            </tr>
                        {/each}
                        </tbody>
                    </table>
                </div>
                {#if selectedChildren.length !== 0}
                <label for="children">Туристы (дети)</label>
                <div id="children" class="overflow-x-auto">
                    <table class="table table-zebra w-full">
                        <thead>
                        <tr>
                            <th class="text-center">Фамилия</th>
                            <th class="text-center">Имя</th>
                            <th class="text-center">Отчество</th>
                            <th class="text-center">Дата рождения</th>
                            <th class="text-center">Пол</th>
                        </tr>
                        </thead>
                        <tbody>
                        {#each selectedChildren as tourist}
                            <tr>
                            <td class="text-center">{tourist.surname}</td>
                            <td class="text-center">{tourist.name}</td>
                            <td class="text-center">{tourist.patronymic}</td>
                            <td class="text-center">{tourist.birthDate.slice(0,10)}</td>
                            <td class="text-center">{tourist.gender}</td>
                            </tr>
                        {/each}
                        </tbody>
                    </table>
                </div>
                {/if}

                <div class="block-block" >
                    <div>
                        <label for="customerSelect" class="label-customer">Заказчик</label>
                        <select id="customerSelect" class="select select-customer select-primary w-full max-w-xs select-center" on:change={handleCustomerChange}>
                            <option disabled selected>Выберите заказчика</option>
                            {#each selectedAdults as tourist}
                                {#if tourist.patronymic === ''}
                                    <option value={tourist.idTourist}>{tourist.surname} {tourist.name[0]}. ({tourist.birthDate.slice(0,10)})</option>
                                {:else}
                                    <option value={tourist.idTourist}>{tourist.surname} {tourist.name[0]}.{tourist.patronymic[0]}. ({tourist.birthDate.slice(0,10)})</option>
                                {/if}
                            {/each}
                        </select>
                    </div>
    
                    <div>
                        <label for="inputCost" class="label-cost">Стоимость</label>
                        <input type="text" id="inputCost" class="input input-bordered input-cost text-center" placeholder=" " title = '' />
                    </div>
                </div>
                

                <h4>ПАРАМЕТРЫ ТУРА</h4>
                <div class="block-block" >
                    <div>
                        <label for="countrySelect" class="label-country">Страна</label>
                        <select id="countrySelect" class="select select-country select-primary w-full max-w-xs select-center" on:change={handleCountryChange} on:change={() => fetchCities(event.target.value)}>
                            <option disabled selected>Выберите страну</option>
                            {#each countries as country}
                                <option value={country.idCountry}>{country.countryName}</option>
                            {/each}
                        </select>
                    </div>

                    <div>
                        <label for="citySelect" class="label-city">Город</label>
                        <select id="citySelect" class="select select-city select-primary w-full max-w-xs select-center" on:change={handleCityChange} on:change={() => fetchResidences(event.target.value)}>
                            <option disabled selected>Выберите город</option>
                            {#each cities as city}
                                <option value={city.idCity}>{city.cityName}</option>
                            {/each}
                        </select>
                    </div>

                    <div>
                        <label for="residenceSelect" class="label-residence">Место проживания</label>
                        <select id="residenceSelect" class="select select-residence select-primary w-full max-w-xs select-center" on:change={handleResidenceChange}>
                            <option disabled selected>Выберите место проживания</option>
                            {#each residences as residence}
                                <option value={residence.idResidence}>{residence.residenceName} {(residence.stars)}</option>
                            {/each}
                        </select>
                    </div>
                </div>
            
                <div class="block-block">
                    <div>
                        <label for="checkinDate" class="label-date-in">Дата заезда</label>
                        <input type="date" id="checkinDate" class="input input-bordered input-date-in" placeholder=" " title = '' />
                    </div>
                    
                    <div>
                        <label for="checkoutDate" class="label-date-out">Дата выезда</label>
                        <input type="date" id="checkoutDate" class="input input-bordered input-date-out" placeholder=" " title = '' />
                    </div>

                    <div>
                        <label for="operatorSelect" class="label-operator">Туроператор</label>
                        <select id="operatorSelect" class="select select-operator select-primary w-full max-w-xs select-center" on:change={handleTourOperatorChange}>
                            <option disabled selected>Выберите туроператора</option>
                            {#each tourOperators as tourOperator}
                                <option value={tourOperator.idTourOperator}>{tourOperator.tourOperatorName}</option>
                            {/each}
                        </select>
                    </div>

                    <div>
                        <label for="typeMealSelect" class="label-type-meal">Тип питания</label>
                        <select id="typeMealSelect" class="select select-type-meal select-primary w-full max-w-xs select-center" on:change={handleTypeMealChange}>
                            <option disabled selected>Выберите тип питания</option>
                            {#each typesMeal as typeMeal}
                                <option value={typeMeal.idTypeMeal}>{typeMeal.typeMealName}</option>
                            {/each}
                        </select>
                    </div>
                </div>
                <button class="add btn btn-ghost" id="toAuth" on:click={addTourClick}>Создать тур</button>
            </form>
        </div>
    </main>
</body>

<style>
    .no-margin {
        margin-top: 0px;
    }
    .block-block {
        display: flex;
        width: 1150px;
		background-color: transparent;
		border-radius: 20px;
        position: static;
		margin-top: 0px;
		z-index: 2;
    }

    .account {
        background-color: rgb(255, 255, 255);
        color: black;
        z-index: 2;
        border-radius: 20px;
    }

    .account:hover {
        background-color: rgb(233, 214, 250);
    }

    .icons_buttons {
        margin-left: 40px;
    }

    .exit {
        margin-left: 250px;
    }

    .border {
        justify-content: center;
        text-align: center;
        display: flex;
        width: 1150px;
        border-radius: 20px;
        background-color: rgb(131, 119, 225);
        position: static;
        margin-top: 20px;
        margin-left: 150px;
        padding: 20px;
    }

    table {
        margin-top: 10px;
        margin-bottom: 10px;
        max-width: 580px;
        font-size: 15px;
    }

    .select {
        border-radius: 20px;
        width: 350px;
        border-width: 2px;
    }

    .select-country {
        margin-left: 20px;
    }

    .select-city {
        margin-left: 30px;
    }

    .select-residence {
        margin-left: 30px;
    }

    .label-country {
        margin-left: 20px;
        width: 350px;
    }

    .label-city {
        margin-left: 30px;
        width: 350px;
    }

    .label-residence {
        margin-left: 30px;
        width: 350px;
    }

    .label-operator {
        width: 300px;
        margin-left: 23.33px;
    }

    .select-operator {
        width: 300px;
        margin-left: 23.33px;
    }

    .label-type-meal {
        margin-left: 23.33px;
        width: 300px;
    }

    .select-type-meal {
        margin-left: 23.33px;
        width: 300px;
    }

    .overflow-x-auto {
        display: flex;
        justify-content: center;
    }

    .select-center {
        justify-content: center;
        text-align: center;
        padding-right: 0;
        padding-left: 0;
    }

    label {
        margin-top: 0px;
        margin-bottom: 5px;
        color: rgb(255, 255, 255);
        font-family: 'Segoe UI';
        font-weight: thin;
    }

    .label-date-in {
        margin-left: 20px;
        width: 220px;
    }

    .input-date-in {
        margin-left: 20px;
        width: 220px;
        border-radius: 20px;
        text-align: center;
        padding-right: 0;
    }

    .input-date-out {
        margin-left: 23.33px;
        width: 220px;
        border-radius: 20px;
        text-align: center;
        padding-right: 0;
    }

    .label-date-out {
        margin-left: 23.33px;
        width: 220px;
    }

    .label-cost {
        margin-left: 20px;
        width: 220px;
    }

    .input-cost {
        margin-left: 20px;
        width: 220px;
        border-radius: 20px;
        text-align: center;
    }

    .label-customer {
        margin-left: 280px;
    }

    .select-customer {
        margin-left: 280px;
        text-align: center;
    }
</style>
