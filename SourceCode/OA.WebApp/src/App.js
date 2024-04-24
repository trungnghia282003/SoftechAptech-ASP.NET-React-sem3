import { BrowserRouter, Route, Routes, useNavigate, useLocation } from 'react-router-dom'
import { useState, useCallback, useMemo, useEffect } from 'react'
import _ from 'lodash'
import { AuthContext } from './contexts/AuthContext'
import AppRoutes from './AppRoutes'
import { Layout } from './components/Layout'
import LoadingPage from './components/atoms/loading-page'
import Header from './components/organisms/header'
import Footer from './components/organisms/footer'

const App = () => {
  return (
    <Layout>
      <BrowserRouter>
        <AppContent />
      </BrowserRouter>
    </Layout>
  )
}

const AppContent = () => {
  const navigate = useNavigate()
  const { pathname } = useLocation()
  const [currentUser, setCurrentUser] = useState(null)
  const [currentModal, setCurrentModal] = useState({})
  const [showLoadingPage, setShowLoadingPage] = useState(false)

  const updateCurrentUser = useCallback(async (nguoiDung) => {
    if (!nguoiDung) {
      setCurrentUser(null)
      return
    }
  }, [])

  const { path: indexRoute } = _.find(AppRoutes, { index: true })

  useEffect(() => {
    if (currentUser) {
      if (currentUser?.role === 2) {
        if (pathname === '/' || pathname === '/login') {
          navigate('/category-management')
        }
        return
      }
      if (pathname === '/' || pathname === '/login') {
        navigate(indexRoute)
      }
      return
    }

    const valueSessionStorage = sessionStorage.getItem('CurrentUser')
    if (valueSessionStorage) {
      setCurrentUser(JSON.parse(valueSessionStorage))
      return
    }
    const valueLocalStorage = localStorage.getItem('CurrentUser')
    if (valueLocalStorage) {
      sessionStorage.setItem('CurrentUser', valueLocalStorage)
      setCurrentUser(JSON.parse(valueLocalStorage))
      return
    }

    if (pathname === '/register') {
      navigate('/register')
      return
    }

    if (pathname === '/forgot-password') {
      navigate('/forgot-password')
      return
    }

    if (pathname === '/category-management') {
      navigate('/category-management')
      return
    }

    navigate('/login')
  }, [currentUser, navigate, pathname, indexRoute])

  const authContextValue = useMemo(
    () => ({
      currentUser,
      updateCurrentUser,
      currentModal,
      setCurrentModal,
      setShowLoadingPage,
    }),
    [currentUser, updateCurrentUser, currentModal, setCurrentModal, setShowLoadingPage],
  )

  return (
    <>
      <AuthContext.Provider value={authContextValue}>
        {currentUser && <Header />}
        <Routes>
          {AppRoutes.map((route, index) => {
            const { element, ...rest } = route
            return <Route key={index} {...rest} element={element} />
          })}
        </Routes>
        {currentUser && <Footer />}
      </AuthContext.Provider>
      {showLoadingPage && <LoadingPage />}
    </>
  )
}

export default App
