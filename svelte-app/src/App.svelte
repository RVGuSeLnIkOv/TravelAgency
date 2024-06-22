<script>
	import { Router, Route } from 'svelte-routing';
	import Auth from './components/Auth.svelte';
	import ChangePass from './components/ChangePass.svelte';
	import DirectorAccount from './components/DirectorAccount.svelte';
	import Employees from './components/Employees.svelte';
	import Tourists from './components/Tourists.svelte';
	import ToursBookings from './components/ToursBookings.svelte';
	import AdministratorAccount from './components/AdministratorAccount.svelte';
	import TourConstructor from './components/TourConstructor.svelte';

	window.history.pushState(null, null, window.location.href);
    window.onpopstate = function(event) {
        window.history.pushState(null, null, window.location.href);
    };
  </script>
  
  <div>
  
	<Router>
	  <Route path="/" component={Auth} />
	  <Route path="/changePass" component={ChangePass} />
	  <Route path="/directorAccount" component={DirectorAccount}/>
	  <Route path="/employees" component={Employees} />
	  <Route path="/tourists" component={Tourists} />
	  <Route path="/toursBookings" component={ToursBookings}/>
	  <Route path="/administratorAccount" component={AdministratorAccount}/>
	  <Route path="/tourConstructor" component={TourConstructor}/>
	</Router>
  </div>
  
  <style>
	
  </style>