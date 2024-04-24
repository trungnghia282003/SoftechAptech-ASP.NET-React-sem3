import _ from 'lodash'
import { React, useState, useContext, useEffect } from 'react'
import { AuthContext } from '../../contexts/AuthContext'
import ItemApi from '../../api/item'
import CategoryApi from '../../api/category'
import Label from '../atoms/forms/label'
import Template from '../templates/default/no-separation-template'
import { useSearchParams } from 'react-router-dom'
import BidDetailSeller from './bid-detail-seller'
import ViewBidDetail from './bid-detail-view'
import PlaceAmount from './bid-detail-place-amount'
import Rating from './rating-detail'
import ProtectedRoute from '../protected-route'

const BidDetail = () => {
  const [item, setItem] = useState(null)
  const [listCategory, setListCategory] = useState([])
  const [mode, setMode] = useState('view')
  const [idItem, setIdItem] = useState('')
  const [titleBidDetailPage, setTitleBidDetailPage] = useState('')
  const [toastImage, settoastImage] = useState('')
  let [searchParams, setSearchParams] = useSearchParams()
  const termIdItem = searchParams.get('id')
  const modeQueryParam = searchParams.get('mode')

  const { currentUser } = useContext(AuthContext)
  let token = null
  let userId = null
  if (currentUser) {
    token = currentUser.token
    userId = currentUser.id
  }

  useEffect(() => {
    if (termIdItem == null) {
      setItem(termIdItem)
    }
    setIdItem(termIdItem)

    setMode(modeQueryParam)
    if (modeQueryParam === 'create') {
      setTitleBidDetailPage('Add new item')
      settoastImage('images/square-plus-solid.svg')
    } else if (modeQueryParam === 'edit') {
      setTitleBidDetailPage('Edit item')
      settoastImage('images/edit.svg')
    } else {
      setTitleBidDetailPage('View item')
      settoastImage('images/bag.svg')
    }

    if (token) {
      const getItem = async () => {
        if (idItem) {
          const item = await ItemApi.getById(idItem, token)
          setItem(item)
        }
      }
      getItem()

      const getListCategory = async () => {
        const result = await CategoryApi.getAllCategory(token, 2)
        setListCategory(result)
      }
      getListCategory()
    }
  }, [token, idItem, modeQueryParam, termIdItem])
  const content = (
    <>
      {mode === 'create' || mode === 'edit' ? (
        <BidDetailSeller titleBidDetailPage={titleBidDetailPage} listCategory={listCategory} item={item} />
      ) : (
        <>
          <div>
            <ViewBidDetail item={item} />
            <PlaceAmount item={item} />
            {item?.userId === userId || item?.buyerBid?.isAchieved === true ? <Rating item={item}></Rating> : null}
          </div>
        </>
      )}
    </>
  )

  return (
    <>
      <ProtectedRoute />
      <Template headerIcon={toastImage} headerTitle={titleBidDetailPage} content={content} />
    </>
  )
}

export default BidDetail
