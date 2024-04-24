import _ from 'lodash'
import Template from '../templates/default/no-separation-template'
import { Container } from 'react-bootstrap'
import './notification.scss'
import Card from 'react-bootstrap/Card'
import Image from '../atoms/image'
import { useContext, useEffect, useState } from 'react'
import { AuthContext } from '../../contexts/AuthContext'
import NotificationApi from '../../api/notification'
import Label from '../atoms/forms/label'
import { useNavigate } from 'react-router-dom'
import ProtectedRoute from '../protected-route'

const Notification = () => {
  const [notifications, setNotifications] = useState([])
  const navigate = useNavigate()
  const { currentUser } = useContext(AuthContext)
  let token = null
  let fullName = null
  let userId = null
  if (currentUser) {
    token = currentUser.token
    fullName = currentUser.fullName
    userId = currentUser.id
  }

  useEffect(() => {
    if (token) {
      const getListNotifications = async () => {
        const result = await NotificationApi.getAllNotificationsByIdUser(userId, token)
        setNotifications(result)
      }
      getListNotifications()
    }
  }, [token, userId])

  let unreadNotifications = []
  let readNotifications = []
  if (notifications) {
    unreadNotifications = notifications.filter((notification) => !notification.markRead)
    unreadNotifications.sort((a, b) => new Date(b.notificationDate) - new Date(a.notificationDate))
    readNotifications = notifications.filter((notification) => notification.markRead)
    readNotifications.sort((a, b) => new Date(b.notificationDate) - new Date(a.notificationDate))
  }

  const handleChangeDate = (date) => {
    const apiDate = new Date(date)
    const options = {
      day: 'numeric',
      month: 'short',
      year: 'numeric',
    }
    return apiDate.toLocaleString('en-GB', options)
  }

  const handleRatingView = (id) => {
    navigate(`/bid-detail?id=${id}&mode=view`)
  }
  const content = (
    <>
      <Container fluid>
        {unreadNotifications.length === 0 && readNotifications.length === 0 ? (
          <Label text='Currently, you have no notifications' />
        ) : null}
        {unreadNotifications.map((notification, index) => (
          <Card
            key={index}
            onClick={() => handleRatingView(notification.itemId)}
            className='notification-card-un-mark my-3'
          >
            <Card.Body className='nofitication-card-body'>
              <Image path='images/bookmark-regular.svg' className={{ bookMarkIcon: true }} />
              <div className='notification-info-card-body'>
                <Card.Text className='username-react-item m-0'>
                  <span className='userName'>{fullName}</span> commented{' '}
                  <span className='itemName'>{notification.itemName}</span>
                </Card.Text>
                <Card.Text className='message m-0'>{notification.message}</Card.Text>
                <Card.Text className='notification-date m-0'>
                  {handleChangeDate(notification.notificationDate)}
                </Card.Text>
              </div>
            </Card.Body>
          </Card>
        ))}

        {readNotifications.map((notification, index) => (
          <Card
            key={index}
            className='notification-card-mark my-3'
            onClick={() => handleRatingView(notification.itemId)}
          >
            <Card.Body className='nofitication-card-body'>
              <Image path='images/bookmark-solid.svg' className={{ bookMarkIcon: true }} />
              <div className='notification-info-card-body'>
                <Card.Text className='username-react-item m-0'>
                  <span className='userName'>{fullName}</span> commented on your recent post{' '}
                  <span className='itemName'>{notification.itemName}</span>
                </Card.Text>
                <Card.Text className='message m-0'>{notification.message}</Card.Text>
                <Card.Text className='notification-date m-0'>
                  {handleChangeDate(notification.notificationDate)}
                </Card.Text>
              </div>
            </Card.Body>
          </Card>
        ))}
      </Container>
    </>
  )

  return (
    <>
      <ProtectedRoute />
      <Template headerIcon={'images/bell-solid.svg'} headerTitle={'Notification page'} content={content} />
    </>
  )
}

export default Notification
