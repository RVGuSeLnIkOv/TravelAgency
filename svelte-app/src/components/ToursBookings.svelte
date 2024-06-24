<script>
    import html2canvas from 'html2canvas';
    import 'tailwindcss/base';
    import 'tailwindcss/components';
    import 'tailwindcss/utilities';
    import 'daisyui/dist/full.css';

    import { navigate } from 'svelte-routing';

    import { onMount } from 'svelte';

    let employee = null;
    let post = '';

    let tours = [];
    let toursBookings = [];

    let successMessage = '';
    let errorMessage = '';
    let index = null;

    const { state } = history;

    if (state && state.employee) {
        employee = state.employee;
        post = employee.post;
    }

    onMount(() => {
        fetchToursBookings();
    });

    function toAuthClick() {
        navigate(`/`);
    }

    function toMainDirectorClick() {
        navigate(`/directorAccount`, { state: { employee } });
    }

    function toMainAdministratorClick() {
        navigate(`/administratorAccount`, { state: { employee } });
    }

    function toTouristsClick() {
        navigate(`/tourists`, { state: { employee } });
    }

    function toEmployeesClick() {
        navigate(`/employees`, { state: { employee } });
    }

    function toToursBookingsClick() {
        navigate(window.location.href, { state: { employee } });
    }

    function closeEForm() {
        errorMessage = '';
    }

    function closeSForm() {
        successMessage = '';
    }

    async function fetchBooking(idTour) {
        try {
            const response = await fetch(`http://localhost:5000/api/Tour/${idTour}/Bookings`);

            if (response.ok) {
                return await response.json();
            } else if (response.status === 404) {
                errorMessage = 'Заказы не найдены';
            } else {
                errorMessage = 'Ошибка при выполнении запроса';
            }

        } catch (error) {
            errorMessage = 'Ошибка при выполнении запроса';
        }
	}

    async function fetchTourists(idTour) {
        try {
            const response = await fetch(`http://localhost:5000/api/Tour/${idTour}/Tourists`);

            if (response.ok) {
                return await response.json();
            } else if (response.status === 404) {
                errorMessage = 'Туристы на туре не найдены';
            } else {
                errorMessage = 'Ошибка при выполнении запроса';
            }

        } catch (error) {
            errorMessage = 'Ошибка при выполнении запроса';
        }
	}

    async function fetchTourist(idTourist) {
        try {
            const response = await fetch(`http://localhost:5000/api/Tourist/${idTourist}`);

            if (response.ok) {
                return await response.json();
            } else if (response.status === 404) {
                errorMessage = 'Турист не найден';
            } else {
                errorMessage = 'Ошибка при выполнении запроса';
            }

        } catch (error) {
            errorMessage = 'Ошибка при выполнении запроса';
        }
	}

    async function fetchEmployee(idEmployee) {
        try {
            const response = await fetch(`http://localhost:5000/api/Employee/${idEmployee}`);

            if (response.ok) {
                return await response.json();
            } else if (response.status === 404) {
                errorMessage = 'Сотрудник не найден';
            } else {
                errorMessage = 'Ошибка при выполнении запроса';
            }

        } catch (error) {
            errorMessage = 'Ошибка при выполнении запроса';
        }
	}

    async function fetchTouristData(idTourist) {
        try {
            const response = await fetch(`http://localhost:5000/api/Tourist/${idTourist}/Data`);

            if (response.ok) {
                return await response.json();
            } else if (response.status === 404) {
                errorMessage = 'Данные туриста не найдены';
            } else {
                errorMessage = 'Ошибка при выполнении запроса';
            }

        } catch (error) {
            errorMessage = 'Ошибка при выполнении запроса';
        }
    }

    async function fetchResidence(idResidence) {
        try {
            const response = await fetch(`http://localhost:5000/api/Residence/${idResidence}`);

            if (response.ok) {
                return await response.json();
            } else {
                errorMessage = 'Ошибка при выполнении запроса';
            }

        } catch (error) {
            errorMessage = 'Ошибка при выполнении запроса';
        }
    }

    async function fetchCity(idCity) {
        try {
            const response = await fetch(`http://localhost:5000/api/City/${idCity}`);

            if (response.ok) {
                return await response.json();
            } else {
                errorMessage = 'Ошибка при выполнении запроса';
            }

        } catch (error) {
            errorMessage = 'Ошибка при выполнении запроса';
        }
    }

    async function fetchCountry(idCountry) {
        try {
            const response = await fetch(`http://localhost:5000/api/Country/${idCountry}`);

            if (response.ok) {
                return await response.json();
            } else {
                errorMessage = 'Ошибка при выполнении запроса';
            }

        } catch (error) {
            errorMessage = 'Ошибка при выполнении запроса';
        }
    }

    async function fetchTourOperator(idTourOperator) {
        try {
            const response = await fetch(`http://localhost:5000/api/TourOperator/${idTourOperator}`);

            if (response.ok) {
                return await response.json();
            } else {
                errorMessage = 'Ошибка при выполнении запроса';
            }

        } catch (error) {
            errorMessage = 'Ошибка при выполнении запроса';
        }
    }

    async function fetchTypeMeal(idTypeMeal) {
        try {
            const response = await fetch(`http://localhost:5000/api/TypeMeal/${idTypeMeal}`);

            if (response.ok) {
                return await response.json();
            } else {
                errorMessage = 'Ошибка при выполнении запроса';
            }

        } catch (error) {
            errorMessage = 'Ошибка при выполнении запроса';
        }
    }

    async function fetchToursBookings() {
        try {
            const response = await fetch(`http://localhost:5000/api/Tour/ActiveTours`);

            if (response.ok) {
                const toursData = await response.json();

                const bookingPromises = toursData.map(tourData => fetchBooking(tourData.idTour));
                const bookingsData = await Promise.all(bookingPromises);

                const touristsPromises = toursData.map(tourData => fetchTourists(tourData.idTour));
                const touristsData = await Promise.all(touristsPromises);

                const residencePromises = toursData.map(tourData => fetchResidence(tourData.idResidence));
                const residences = await Promise.all(residencePromises);

                const cityPromises = residences.map(residence => fetchCity(residence.idCity));
                const cities = await Promise.all(cityPromises);

                const customerPromises = bookingsData.map(bookingData => fetchTourist(bookingData[0].idTourist));
                const customers = await Promise.all(customerPromises);

                const employeePromises = bookingsData.map(bookingData => fetchEmployee(bookingData[0].idEmployee));
                const employees = await Promise.all(employeePromises);

                const countryPromises = cities.map(city => fetchCountry(city.idCountry));
                const countries = await Promise.all(countryPromises);

                const tourOperatorPromises = toursData.map(tourData => fetchTourOperator(tourData.idTourOperator));
                const tourOperators = await Promise.all(tourOperatorPromises);

                const typeMealPromises = toursData.map(tourData => fetchTypeMeal(tourData.idTypeMeal));
                const typeMeals = await Promise.all(typeMealPromises);

                const touristsDetailsPromises = touristsData.map(async tourists => {
                    const touristsDetails = [];
                    for (const tourist of tourists) {
                        const touristDetails = await fetchTouristData(tourist.idTourist);
                        touristsDetails.push(touristDetails);
                    }
                    return touristsDetails;
                });
                const touristsDetailsData = await Promise.all(touristsDetailsPromises);

                toursBookings = toursData.map((tourData, index) => {
                const bookingData = bookingsData[index];
                const tourists = touristsData[index];
                const touristsDetails = touristsDetailsData[index];

                const residence = residences[index];
                const city = cities[index];
                const country = countries[index];
                const tourOperator = tourOperators[index];
                const typeMeal = typeMeals[index];
                const customer = customers[index];
                const employee = employees[index];

                return {
                    idTour: tourData.idTour,
                    residence: {
                        idResidence: residence.idResidence,
                        residenceName: residence.residenceName,
                        city: {
                            idCity: city.idCity,
                            cityName: city.cityName,
                            country: {
                                idCountry: country.idCountry,
                                countryName: country.countryName
                            }
                        }
                    },
                    tourOperator: {
                        idTourOperator: tourOperator.idTourOperator,
                        tourOperatorName: tourOperator.tourOperatorName
                    },
                    typeMeal: {
                        idTypeMeal: typeMeal.idTypeMeal,
                        typeMealName: typeMeal.typeMealName,
                        typeMealAbbrName: typeMeal.typeMealAbbrName
                    },
                    checkinDate: tourData.checkinDate,
                    checkoutDate: tourData.checkoutDate,
                    duration: tourData.duration,
                    customer: {
                        surname: customer.surname,
                        name: customer.name,
                        patr: customer.patronymic
                    },
                    employee: {
                        surname: employee.surname,
                        name: employee.name,
                        patr: employee.patronymic
                    },
                    date: bookingData[0].date,
                    cost: bookingData[0].cost,
                    tourists: tourists.map((tourist, i) => ({
                        idTourist: tourist.idTourist,
                        surname: tourist.surname,
                        name: tourist.name,
                        patr: tourist.patronymic,
                        surnameF: tourist.foreignSurname,
                        nameF: tourist.foreignSurname,
                        patrF: tourist.foreignPatromymic,
                        birth: tourist.birthDate,
                        gender: tourist.gender,
                        isChild: tourist.isChild,
                        address: touristsDetails[i].address,
                        email: touristsDetails[i].email,
                        phone: touristsDetails[i].phoneNumber,
                        internal: touristsDetails[i].internalPassportNumber,
                        foreign: touristsDetails[i].foreignPassportNumber,
                        certificate: touristsDetails[i].birthCertificateNumber
                    }))
                };
            });

            } else if (response.status === 404) {
            errorMessage = 'Туры не найдены';
            } else {
            errorMessage = 'Ошибка при выполнении запроса';
            }
        } catch (error) {
            errorMessage = 'Ошибка при выполнении запроса';
        }
	}

    async function deleteTour(tourBooking) {
        const deleteTour = {
            idTour: tourBooking.idTour,
            idTourOperator: tourBooking.tourOperator.idTourOperator,
            idResidence: tourBooking.residence.idResidence,
            idTypeMeal: tourBooking.typeMeal.idTypeMeal,
            checkinDate: tourBooking.checkinDate,
            checkoutDate: tourBooking.checkoutDate,
            duration: tourBooking.duration
        }

        try {
            const response = await fetch(`http://localhost:5000/api/Tour/${tourBooking.idTour}/Archive`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json; charset=utf-8'
            },
            body: JSON.stringify(deleteTour)
            });

            if (response.ok) {
                successMessage = 'Тур и связанная с ним информация успешно удалены!';
                fetchToursBookings();
            } else {
                errorMessage = 'Ошибка при выполнении запроса';
            }
        } catch (error) {
            errorMessage = 'Ошибка при выполнении запроса';
        }
    }
    

    function downloadTour(indexTour){
        index = indexTour;
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
        {#if post === "Директор"}
            <div class="navbar bg-base-100">
                <div>
                    <span>
                        <h3>ДИРЕКТОР</h3>
                        <h3>{employee.surname} {employee.name[0]}.</h3>
                    </span>
                </div>
                <div class="icons_buttons">
                    <button class="account btn btn-ghost" id="currentLink" on:click={toMainDirectorClick}>
                        <img src="/home.png" alt="Главная" class="icon" />
                        Главная
                    </button>
                    <button class="account btn btn-ghost" on:click={toEmployeesClick}>
                        <img src="/employee.png" alt="Сотрудники" class="icon" />
                        Сотрудники
                    </button>
                    <button class="account btn btn-ghost" on:click={toTouristsClick}>
                        <img src="/tourist.png" alt="Туристы" class="icon" />
                        Туристы
                    </button>
                    <button class="account active btn btn-ghost" on:click={toToursBookingsClick}>
                        <img src="/word.png" alt="Туры и заказы" class="icon" />
                        Туры и заказы
                    </button>
                </div>
                <button class="exit btn btn-ghost" id="toAuth" on:click={toAuthClick}>Выход</button>
            </div>
        {/if}

        {#if post === "Администратор"}
            <div class="navbar bg-base-100">
                <div>
                    <span>
                        <h3>АДМИНИСТРАТОР</h3>
                        <h3>{employee.surname} {employee.name[0]}.</h3>
                    </span>
                </div>
                <div class='icons_buttons'>
                    <button class="account btn btn-ghost" id="currentLink" on:click={toMainAdministratorClick}>
                        <img src="/home.png" alt="Главная" class="icon" />
                        Главная
                    </button>
                    <button class="account btn btn-ghost" on:click={toTouristsClick}>
                        <img src="/tourist.png" alt="Туристы" class="icon" />
                        Туристы
                    </button>
                    <button class="account active btn btn-ghost" on:click={toToursBookingsClick}>
                        <img src="/word.png" alt="Туры и заказы" class="icon" />
                        Туры и заказы
                    </button>
                </div>
                <button class="exit btn btn-ghost" id="toAuth" on:click={toAuthClick}>Выход</button>
            </div>
        {/if}

        <div class="overflow-x-auto">
            <table class="table">
                <thead>
                <tr>
                    <th class="text-center">Город</th>
                    <th class="text-center">Место проживания</th>
                    <th class="text-center">Дата заселения</th>
                    <th class="text-center">Дата выселения</th>
                    <th class="text-center">Заказчик</th>
                    <th class="text-center">Сотрудник</th>
                    <th class="text-center">Стоимость</th>
                    <th class="text-center">Дата оформления</th>
                    <th class="text-center">Удаление</th>
                </tr>
                </thead>
                <tbody>
                {#each toursBookings as tourBooking}
                    <tr>
                    <td class="text-center">{tourBooking.residence.city.cityName}</td>
                    <td class="text-center">{tourBooking.residence.residenceName}</td>
                    <td class="text-center">{tourBooking.checkinDate.slice(0,10)}</td>
                    <td class="text-center">{tourBooking.checkoutDate.slice(0,10)}</td>
                    <td class="text-center">{tourBooking.customer.surname} {tourBooking.customer.name[0]}.{tourBooking.customer.patr[0]}.</td>
                    <td class="text-center">{tourBooking.employee.surname} {tourBooking.employee.name[0]}.{tourBooking.employee.patr[0]}.</td>
                    <td class="text-center">{tourBooking.cost}</td>
                    <td class="text-center">{tourBooking.date.slice(0,10)}</td>
                    <td class="text-center"><button class="btn btn-outline btn-error" on:click={() => deleteTour(tourBooking)}>Удалить</button></td>
                    </tr>
                {/each}
                </tbody>
            </table>
        </div>
        {#if index !== null}
        <div class="myId">
            <h1 class="doc">Договор на оказание туристических услуг</h1>
            <p>
                Турагент ООО "Райский берег" в лице директора, действующего на основании Устава (далее –
Исполнитель), действующий по поручению Туроператора ООО "Алеан Отдых", реестровый номер РТО022481 на основании
договора на продвижение и реализацию туристского продукта , заключенного между туроператором и турагентом (далее –
также Туроператор), с одной стороны, и Турист и (или) иной Заказчик1 АВЕРИНА ЕКАТЕРИНА СЕРГЕЕВНА (далее соответственно -
Заказчик, Турист), с другой стороны, вместе именуемые «Стороны», заключили настоящий договор (далее – Договор) о
нижеследующем:
            </p>
        </div>
        {/if}
    </main>
</body>


<style>
    .account {
        background-color: rgb(255, 255, 255);
        color: black;
        z-index: 2;
        border-radius: 20px;
    }

    .active {
        background-color: rgb(218, 204, 232);
        color: black;
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

    .overflow-x-auto {
        display: flex;
        justify-content: center;
    }
</style>