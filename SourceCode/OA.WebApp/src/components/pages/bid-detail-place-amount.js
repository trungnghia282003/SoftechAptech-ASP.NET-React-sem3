import _ from 'lodash'
import { React, useContext } from 'react'
import { Form } from 'react-bootstrap'
import { AuthContext } from '../../contexts/AuthContext'
import * as yup from 'yup'
import { useFormik } from 'formik'
import bidApi from '../../api/bid'
import Input from '../atoms/forms/input'
import Button from '../molecules/buttons/button'
import './bid-detail-place-amount.scss'
import Label from '../atoms/forms/label'
import { useNavigate } from 'react-router-dom'
import NumberField from '../molecules/forms/number-field'
import { errorMessages } from '../../common'

const PlaceAmount = ({ item }) => {
  const navigate = useNavigate()
  const { currentUser } = useContext(AuthContext)
  let userId = null
  let roleUser = null
  let token = null
  if (currentUser) {
    userId = currentUser.id
    roleUser = currentUser.role
    token = currentUser.token
  }
  const formik = useFormik({
    initialValues: {
      id: 0,
      buyerId: userId,
      itemId: item?.id || 0,
      amount: '',
    },
    enableReinitialize: true,
    validationSchema: yup.object().shape({
      id: yup.number(),
      buyerId: yup.number(),
      itemId: yup.number(),
      amount: yup
        .number()
        .min(item?.currentBidPrice + item?.minIncreasePrice, errorMessages.amount.isValid)
        .integer()
        .required(errorMessages.amount.required),
    }),
  })

  const handleCreateBid = async () => {
    const data = formik.values
    if (data.amount != 0) {
      await bidApi.create(data, token)
      navigate(`/bid-detail?id=${data.itemId}&mode=viewamount`)
    }
  }
  return (
    <Form noValidate onSubmit={formik.handleSubmit}>
      <div>
        {userId !== item?.sellerId && item?.bidStatus === 1 && roleUser === 0 && item?.buyerBid === null ? (
          <>
            <div className='place-bid-bdpage'>
              <NumberField label='Your bid' name='amount' {...formik} />
              <Button
                className={'place-bid-btn-bdpage mt-2'}
                variant='warning'
                type='submit'
                text='Place bid'
                onClick={handleCreateBid}
                disabled={!(formik.isValid && formik.dirty)}
              />
            </div>
          </>
        ) : null}
      </div>
    </Form>
  )
}

export default PlaceAmount
