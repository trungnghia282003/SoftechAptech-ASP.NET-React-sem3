import React, { useState, useContext, useEffect, useRef } from 'react'
import { useNavigate, useSearchParams } from 'react-router-dom'
import Stack from 'react-bootstrap/Stack'
import UserInformation from './user-information'
import './index.scss'
import { AuthContext } from '../../../contexts/AuthContext'
import SignOutModal from '../../atoms/modals/confirmation'
import { confirmMessages } from '../../../common'
import { Nav, Navbar } from 'react-bootstrap'
import CategoryApi from '../../../api/category'
import SearchBoxHeader from './search-box'
import NotificationApi from '../../../api/notification'

function Header() {
  const [showLogOutModel, setShowLogOutModel] = useState(false)
  const [listCategory, setListCategory] = useState([])
  const { currentUser, updateCurrentUser } = useContext(AuthContext)
  const [notifications, setNotifications] = useState([])
  const [countNotifications, setCountNotifications] = useState(null)
  const navigate = useNavigate()

  let token = null
  let userId = null
  if (currentUser) {
    token = currentUser.token
    userId = currentUser.id
  }

  useEffect(() => {
    if (token) {
      const getListCategory = async () => {
        const result = await CategoryApi.getAllCategoryActive(token)
        setListCategory(result)
      }
      getListCategory()

      const getListNotifications = async () => {
        const result = await NotificationApi.getAllNotificationsByIdUser(userId, token)
        handleFilterNotifications(result)
      }
      getListNotifications()
    }
  }, [token, userId])

  const handleLogOut = () => {
    setShowLogOutModel(true)
  }

  const handleConfirmLogout = async () => {
    sessionStorage.removeItem('CurrentUser')
    localStorage.removeItem('CurrentUser')
    await updateCurrentUser(null)
    setShowLogOutModel(false)
    navigate('/login')
  }

  const handleCreateItem = () => {
    navigate('/bid-detail?mode=create')
  }

  const handleCategoryBrowser = (categoryName) => {
    const foundCategory = listCategory.find((item) => item.name === categoryName)
    navigate(`/category-browser?id=${foundCategory.id}`)
  }

  const handleFilterNotifications = (data) => {
    const foundNotifications = data.filter((notification) => notification.markRead === false)
    let maxIndex = 0
    foundNotifications.map((item, index) => {
      if (index >= maxIndex) {
        maxIndex = index + 1
      }
    })
    setCountNotifications(maxIndex)
    setNotifications(foundNotifications)
  }

  const handleClick = async (item) => {
    if (!item) {
      return
    }

    if (item.name === 'notification') {
      await NotificationApi.updateNotification(token)
    }
    setCountNotifications(0)
    navigate(item.path)
  }

  return (
    <>
      <Stack direction='horizontal' className='header-container'>
        <SearchBoxHeader></SearchBoxHeader>
        <UserInformation
          maxIndex={countNotifications}
          handleClick={handleClick}
          text={currentUser?.fullName}
          handleDangXuat={handleLogOut}
          role={currentUser?.role}
          currentUser={currentUser}
          handleCreateItem={handleCreateItem}
        ></UserInformation>
      </Stack>
      <Navbar bg='dark' data-bs-theme='dark' className='d-flex justify-content-center'>
        {currentUser?.role !== 2 ? (
          <div>
            <Nav className='me-auto'>
              {listCategory.map((item, index) => (
                <Nav.Link key={index} className='px-4  text-white' onClick={() => handleCategoryBrowser(item.name)}>
                  {item.name}
                </Nav.Link>
              ))}
            </Nav>
          </div>
        ) : null}
      </Navbar>
      <SignOutModal
        body={confirmMessages.signOut}
        show={showLogOutModel}
        onHide={() => setShowLogOutModel(false)}
        onClick={handleConfirmLogout}
      />
    </>
  )
}

export default Header
