import _ from 'lodash'
import { AuthContext } from '../../contexts/AuthContext'
import { useContext, useState, useEffect } from 'react'
import { useNavigate } from 'react-router-dom'
import ItemApi from '../../api/item'
import './home.scss'
import Image from '../atoms/image'
import Carousel from 'react-bootstrap/Carousel'
import { Col, Row } from 'react-bootstrap'
import { Container } from 'reactstrap'
import ViewItemDetailLabel from './item-detail-label'
import ProtectedRoute from '../protected-route'
import Body from '../organisms/body'
import Label from '../atoms/forms/label'

const Home = () => {
  const [listItems, setListItems] = useState([])

  const navigate = useNavigate()
  const { currentUser } = useContext(AuthContext)
  let token = null
  if (currentUser) {
    token = currentUser.token
  }

  const carouselItems = [
    {
      id: 1,
      path: 'images/Slider5.jpg',
      value:
        'The category of Art includes classical art, modern & contemporary art, photography, posters, prints & limited editions.',
    },
    {
      id: 6,
      path: 'images/Slider4.jpg',
      value:
        'The category Coins & Stamps includes ancient coins, banknotes, bullion, euro coins, modern coins, postcards, stamps, and world coins.',
    },
    {
      id: 8,
      path: 'images/Slider2.jpg',
      value:
        'The category Car & Motorbikes includes classic cars, classic motorcycles & scooters, automobilia & motobilia.',
    },
  ]

  useEffect(() => {
    if (token) {
      const getListItems = async () => {
        const result = await ItemApi.getAllItem(token)
        setListItems(result)
      }
      getListItems()
    }
  }, [token])

  const upcomingItem = (array) => {
    const bidStatusArray = []
    array.map((item) => {
      if (item.bidStatus === 0) {
        bidStatusArray.push(item)
      }
    })
    return bidStatusArray
  }

  const happeningItem = (array) => {
    const bidStatusArray = []
    array.map((item) => {
      if (item.bidStatus === 1) {
        bidStatusArray.push(item)
      }
    })
    return bidStatusArray
  }

  const handleBrowseCategory = (id) => {
    navigate(`/category-browser?id=${id}`)
  }

  const content = (
    <>
      <div className='banner'>
        <h4 className='banner-heading'>
          Find big brands at bargain prices right here, the home of consumer goods and retail surplus auctions
        </h4>
      </div>
      <Container fluid>
        <Col className='status-auctions py-1'>
          <h5 className='status-auctions-title'>Happening auctions</h5>
          <hr className='singleline m-0 ' />
          <div className='view-item-homepage'>
            <ViewItemDetailLabel items={happeningItem(listItems)} />
          </div>
        </Col>
        <Col className='status-auctions py-1'>
          <h5 className='status-auctions-title'>Upcoming auctions</h5>
          <hr className='singleline m-0' />
          <div className='view-item-homepage'>
            <ViewItemDetailLabel items={upcomingItem(listItems)} />
          </div>
        </Col>
        <Row className='image-horizontal mb-4'>
          <Carousel>
            {carouselItems.map((item, index) => (
              <Carousel.Item key={index}>
                <Label
                  onClick={() => handleBrowseCategory(item.id)}
                  text='See More'
                  className={{ 'label-carousel-category-menu': true }}
                />
                <Image path={item.path} className={{ carouselImage: true }} />
              </Carousel.Item>
            ))}
          </Carousel>
        </Row>
      </Container>
    </>
  )

  return (
    <>
      <ProtectedRoute />
      <Body className={{ 'body-layout': true }} children={content} />
    </>
  )
}

export default Home
