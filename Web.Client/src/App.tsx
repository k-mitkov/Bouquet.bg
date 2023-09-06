import Navigation from './components/Navigation';
import { Spinner } from './components/utilities';
import Routes from './routes/Routes';
import { useCities } from './stateProviders/cachedDataContext/DataContext';
import { useCity } from './stateProviders/cityContext';
import { useUser } from './stateProviders/userContext';



function App() {
  const { isCityLoaded } = useCity();
  const { isCitiesLoaded, isColorsLoaded, isFlowersLoaded } = useCities();
  const { isUserLoaded } = useUser();

  if (!isCityLoaded || !isCitiesLoaded || !isColorsLoaded || !isFlowersLoaded || !isUserLoaded) {
    return (
      <>
        <div className='flex w-full h-auto overflow-hidden items-center justify-center'>
          <Spinner />
        </div>
      </>
    )
  }


  return (
    <div className="h-full min-h-screen bg-[url('./resources/images/bg-flowers.jpg')] dark:bg-[url('./resources/images/dark-bg-flowers.jpg')] overflow-y-auto">
      <Navigation />
      <div className='md:grid md:grid-cols-12'>
        <div className='md:col-start-2 md:col-span-10'>
          <Routes />
        </div>
      </div>
    </div>
  )
}

export default App
